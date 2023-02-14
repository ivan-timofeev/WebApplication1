using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Common.SearchEngine;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Models;
using WebApplication1.Common.SearchEngine.Models;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories;

public class ManufacturersRepository : IManufacturersRepository
{
    private readonly WebApplicationDbContext _dbContext;
    private readonly ISearchEngine _searchEngine;

    public ManufacturersRepository(
        WebApplicationDbContext dbContext,
        ISearchEngine searchEngine)
    {
        _dbContext = dbContext;
        _searchEngine = searchEngine;
    }

    public Manufacturer Create(Manufacturer entity)
    {
        _dbContext.Manufacturers.Add(entity);
        _dbContext.SaveChanges();

        return entity;
    }

    public Manufacturer Read(Guid id)
    {
        var manufacturer = GetManufacturersSource()
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        if (manufacturer is null)
        {
            throw new EntityNotFoundInTheDatabaseException(id);
        }

        return manufacturer;
    }

    public Manufacturer Update(Guid entityId, Manufacturer newEntityState)
    {
        var manufacturer = GetManufacturersSource()
            .FirstOrDefault(x => x.Id == entityId);
        
        if (manufacturer is null)
        {
            throw new EntityNotFoundInTheDatabaseException(entityId);
        }

        manufacturer.Name = newEntityState.Name;

        _dbContext.SaveChanges();

        return manufacturer;
    }

    public void Delete(Guid entityId)
    {
        var manufacturer = GetManufacturersSource().FirstOrDefault(x => x.Id == entityId);
        
        if (manufacturer is null)
        {
            throw new EntityNotFoundInTheDatabaseException(entityId);
        }
        
        manufacturer.IsDeleted = true;
        manufacturer.DeletedDateTimeUtc = DateTime.UtcNow;
        
        _dbContext.SaveChanges();
    }

    public IEnumerable<Manufacturer> Read(IEnumerable<Guid> ids)
    {
        var manufacturers = GetManufacturersSource()
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking()
            .ToArray();

        var notFoundEntitiesIds = ids.Except(manufacturers.Select(x => x.Id));

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(notFoundEntitiesIds.ToArray());
        }

        return manufacturers.ToArray();
    }

    public PagedModel<Manufacturer> ReadWithPagination(IEnumerable<Guid> ids, int page, int pageSize)
    {
        var manufacturers = GetManufacturersSource()
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking();

        var notFoundEntitiesIds = ids.Except(manufacturers.Select(x => x.Id));

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(notFoundEntitiesIds.ToArray());
        }

        return PagedModel<Manufacturer>.Paginate(manufacturers, page, pageSize);
    }

    public IEnumerable<Manufacturer> Search(SearchEngineFilter? filter)
    {
        return _searchEngine
            .ExecuteEngine(GetManufacturersSource(), filter)
            .AsNoTracking()
            .ToArray();
    }

    public PagedModel<Manufacturer> SearchWithPagination(SearchEngineFilter? filter, int page, int pageSize)
    {
        var manufacturers = _searchEngine
            .ExecuteEngine(GetManufacturersSource(), filter)
            .AsNoTracking();

        return PagedModel<Manufacturer>.Paginate(manufacturers, page, pageSize);
    }

    private IQueryable<Manufacturer> GetManufacturersSource()
    {
        return _dbContext.Manufacturers;
    }
}

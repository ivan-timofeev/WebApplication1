using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstractions.Data;
using WebApplication1.Abstractions.Data.Repositories;
using WebApplication1.Abstractions.Models;
using WebApplication1.Abstractions.Services.SearchEngine;
using WebApplication1.Common.Exceptions;
using WebApplication1.Models;
using WebApplication1.Services.SearchEngine.Models;

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

    public Guid Create(Manufacturer entity)
    {
        _dbContext.Manufacturers.Add(entity);
        _dbContext.SaveChanges();

        return entity.Id;
    }

    public Manufacturer Read(Guid id)
    {
        var manufacturer = GetManufacturersSource()
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        if (manufacturer is null)
        {
            throw new EntityNotFoundInTheDatabaseException(
                nameof(Manufacturer), id);
        }

        return manufacturer;
    }

    public void Update(Guid entityId, Manufacturer newEntityState)
    {
        var manufacturer = GetManufacturersSource()
            .FirstOrDefault(x => x.Id == entityId);
        
        if (manufacturer is null)
        {
            throw new EntityNotFoundInTheDatabaseException(
                nameof(Manufacturer), entityId);
        }

        manufacturer.Name = newEntityState.Name;

        _dbContext.SaveChanges();
    }

    public void Delete(Guid entityId)
    {
        var manufacturer = GetManufacturersSource().FirstOrDefault(x => x.Id == entityId);
        
        if (manufacturer is null)
        {
            throw new EntityNotFoundInTheDatabaseException(
                nameof(Manufacturer), entityId);
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

        var notFoundEntitiesIds = ids
            .Except(manufacturers.Select(x => x.Id))
            .ToArray();

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(
                nameof(Manufacturer), notFoundEntitiesIds);
        }

        return manufacturers.ToArray();
    }

    public PagedModel<Manufacturer> ReadWithPagination(IEnumerable<Guid> ids, int page, int pageSize)
    {
        var manufacturers = GetManufacturersSource()
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking();

        var notFoundEntitiesIds = ids
            .Except(manufacturers.Select(x => x.Id))
            .ToArray();

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(
                nameof(Manufacturer), notFoundEntitiesIds);
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

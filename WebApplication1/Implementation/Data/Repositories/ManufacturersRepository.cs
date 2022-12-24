using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Models;
using WebApplication1.Common.SearchEngine.Abstractions;
using WebApplication1.Models;

namespace WebApplication1.Data.Repositories;

public class ManufacturersRepository : IRepository<Manufacturer>
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
            .AsNoTracking()
            .ToArray();

        var notFoundEntitiesIds = ids.Except(manufacturers.Select(x => x.Id));

        if (notFoundEntitiesIds.Any())
        {
            throw new OneOrMoreEntitiesNotFoundInTheDatabaseException(notFoundEntitiesIds.ToArray());
        }

        return new PagedModel<Manufacturer>(manufacturers, page, pageSize, manufacturers.Count());
    }

    public IEnumerable<Manufacturer> Search(string? searchQuery)
    {
        return _searchEngine
            .ExecuteEngine(GetManufacturersSource(), searchQuery ?? "")
            .ToArray();
    }

    public PagedModel<Manufacturer> SearchWithPagination(string? searchQuery, int page, int pageSize)
    {
        var manufacturers = _searchEngine
            .ExecuteEngine(GetManufacturersSource(), searchQuery ?? string.Empty)
            .ToArray();

        return new PagedModel<Manufacturer>(manufacturers, page, pageSize, manufacturers.Length);
    }

    private IQueryable<Manufacturer> GetManufacturersSource()
    {
        return _dbContext.Manufacturers;
    }
}
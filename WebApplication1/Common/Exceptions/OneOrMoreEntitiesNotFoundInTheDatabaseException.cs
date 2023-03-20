using WebApplication1.Abstractions.Common;
using WebApplication1.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Common.Exceptions;

public sealed class OneOrMoreEntitiesNotFoundInTheDatabaseException
    : Exception, ICanBeProcessedByErrorMiddleware
{
    private readonly string _entityType;
    private readonly Guid[] _entitiesIds;
    
    public OneOrMoreEntitiesNotFoundInTheDatabaseException(string entityType, params Guid[] entitiesIds)
        : base("Entity was not found in the database.")
    {
        _entityType = entityType;
        _entitiesIds = entitiesIds;
        Data["EntityType"] = _entityType;
        Data["EntitiesIds"] = _entitiesIds;
    }
    
    public ErrorVm GetErrorVm()
    {
        var errorVmBuilder = new ErrorVmBuilder();

        foreach (var entityId in _entitiesIds)
        {
            errorVmBuilder.WithError("Id", "Entity was not found in the database.", entityId.ToString());
        }

        return errorVmBuilder
            .WithInfo("EntityType", _entityType)
            .Build();
    }
    
    public int GetHttpStatusCode()
    {
        return 404;
    }
}
using WebApplication1.Abstraction.Common.Exceptions;
using WebApplication1.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Common.Exceptions;

public sealed class EntityNotFoundInTheDatabaseException
    : Exception, ICanBeProcessedByErrorMiddleware
{
    private readonly Guid _entityId;
    private readonly string _entityType;
    
    public EntityNotFoundInTheDatabaseException(string entityType, Guid entityId)
        : base("Entity was not found in the database.")
    {
        _entityId = entityId;
        _entityType = entityType;
        Data["EntityId"] = entityId;
        Data["EntityType"] = entityType;
    }

    public ErrorVm GetErrorVm()
    {
        return new ErrorVmBuilder()
            .WithError("Id", "Entity was not found in the database.",_entityId.ToString())
            .WithInfo("EntityType", _entityType)
            .Build();
    }

    public int GetHttpStatusCode()
    {
        return 404;
    }
}
using WebApplication1.Abstraction.Common.Exceptions;
using WebApplication1.ViewModels;

namespace WebApplication1.Common.Exceptions;

public sealed class EntityNotFoundInTheDatabaseException : Exception, IErrorVmProvider
{
    private Guid _entityId;
    
    public EntityNotFoundInTheDatabaseException(Guid entityId)
        : base("Entity was not found in the database.")
    {
        _entityId = entityId;
        Data["EntityId"] = entityId;
    }

    public ErrorVm GetErrorVm()
    {
        return new ErrorVmBuilder()
            .WithError("Id", "Entity was not found in the database.",_entityId.ToString())
            .Build();
    }

    public int GetHttpStatusCode()
    {
        return 404;
    }
}
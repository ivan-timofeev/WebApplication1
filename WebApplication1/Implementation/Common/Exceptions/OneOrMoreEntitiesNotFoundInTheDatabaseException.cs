using WebApplication1.Implementation.ViewModels;

namespace WebApplication1.Data.Repositories;

public sealed class OneOrMoreEntitiesNotFoundInTheDatabaseException : Exception, IErrorVmProvider
{
    private readonly Guid[] _entitiesIds;
    
    public OneOrMoreEntitiesNotFoundInTheDatabaseException(params Guid[] entitiesIds)
        : base("Entity was not found in the database.")
    {
        _entitiesIds = entitiesIds;
        Data["EntitiesIds"] = _entitiesIds;
    }
    
    public ErrorVm GetErrorVm()
    {
        var errorVm = new ErrorVm();

        foreach (var entityId in _entitiesIds)
        {
            errorVm.AddError("Id", "Entity was not found in the database.", entityId.ToString());
        }

        return errorVm;
    }
    
    public int GetHttpStatusCode()
    {
        return 404;
    }
}
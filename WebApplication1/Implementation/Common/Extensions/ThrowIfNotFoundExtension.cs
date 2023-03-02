using WebApplication1.Common.Exceptions;

namespace WebApplication1.Helpers.Extensions;

public static class ThrowIfNotFoundExtension
{
    public static T ThrowIfNotFound<T>(this T? entity, Guid entityId)
        where T : class
    {
        if (entity is null)
            throw new EntityNotFoundInTheDatabaseException(
                typeof(T).Name, entityId);
        
        return entity;
    }
}

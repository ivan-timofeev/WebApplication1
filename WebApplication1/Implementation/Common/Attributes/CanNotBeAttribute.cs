using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Attributes;

public class CanNotBeAttribute : ValidationAttribute
{
    private readonly Type _type;
 
    public CanNotBeAttribute(Type type)
    {
        _type = type;
    }
    
    public override bool IsValid(object? value)
    {
        return value is not IEnumerable casted
               || casted.Cast<object?>().All(t => t?.GetType() != _type);
    }
}
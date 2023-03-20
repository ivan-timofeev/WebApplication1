using WebApplication1.Abstractions.Common;
using WebApplication1.ViewModels;

namespace WebApplication1.Common.Exceptions;

public class SearchEngineFilterValidationException
    : Exception, ICanBeProcessedByErrorMiddleware
{
    public SearchEngineFilterValidationException(string message)
        : base(message)
    {
        
    }
    
    public ErrorVm GetErrorVm()
    {
        throw new NotImplementedException();
    }

    public int GetHttpStatusCode()
    {
        throw new NotImplementedException();
    }
}
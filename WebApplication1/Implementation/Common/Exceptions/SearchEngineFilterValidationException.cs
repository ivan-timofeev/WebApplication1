using WebApplication1.Abstraction.Common.Exceptions;
using WebApplication1.ViewModels;

namespace WebApplication1.Common.Exceptions;

public class SearchEngineFilterValidationException : Exception, IErrorVmProvider
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
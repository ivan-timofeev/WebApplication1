using WebApplication1.Data.Repositories;
using WebApplication1.Implementation.ViewModels;

namespace WebApplication1.Common.SearchEngine;

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
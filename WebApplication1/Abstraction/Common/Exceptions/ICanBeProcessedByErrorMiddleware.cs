using WebApplication1.ViewModels;

namespace WebApplication1.Abstraction.Common.Exceptions;

public interface ICanBeProcessedByErrorMiddleware
{
    ErrorVm GetErrorVm();
    int GetHttpStatusCode();
}

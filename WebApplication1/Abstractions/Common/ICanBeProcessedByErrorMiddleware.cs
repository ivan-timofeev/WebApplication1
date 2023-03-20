using WebApplication1.ViewModels;

namespace WebApplication1.Abstractions.Common;

public interface ICanBeProcessedByErrorMiddleware
{
    ErrorVm GetErrorVm();
    int GetHttpStatusCode();
}

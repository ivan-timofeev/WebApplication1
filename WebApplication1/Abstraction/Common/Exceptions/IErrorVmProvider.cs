using WebApplication1.ViewModels;

namespace WebApplication1.Abstraction.Common.Exceptions;

public interface IErrorVmProvider
{
    ErrorVm GetErrorVm();
    int GetHttpStatusCode();
}

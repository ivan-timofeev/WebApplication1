using WebApplication1.Implementation.ViewModels;

namespace WebApplication1.Data.Repositories;

public interface IErrorVmProvider
{
    ErrorVm GetErrorVm();
    int GetHttpStatusCode();
}

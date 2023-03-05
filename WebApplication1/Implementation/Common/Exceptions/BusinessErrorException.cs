using WebApplication1.Abstraction.Common.Exceptions;
using WebApplication1.ViewModels;

namespace WebApplication1.Common.Exceptions;

public sealed class BusinessErrorException
    : Exception, ICanBeProcessedByErrorMiddleware
{
    private readonly int _httpCode;
    private readonly ErrorVm _errorVm;

    public BusinessErrorException(string message, int httpCode, ErrorVm errorVm)
        : base(message)
    {
        _httpCode = httpCode;
        _errorVm = errorVm;
    }

    public ErrorVm GetErrorVm()
    {
        return _errorVm;
    }

    public int GetHttpStatusCode()
    {
        return _httpCode;
    }
}

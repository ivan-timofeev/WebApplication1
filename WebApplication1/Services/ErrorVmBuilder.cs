using System.Collections.Immutable;
using WebApplication1.ViewModels;

namespace WebApplication1.Services;

public class ErrorVmBuilder
{
    private Dictionary<string, List<ErrorElement>> _errors;
    private Dictionary<string, string> _additionalInformation;

    public ErrorVmBuilder()
    {
        _errors = new Dictionary<string, List<ErrorElement>>();
        _additionalInformation = new Dictionary<string, string>();
    }
    
    public ErrorVm Build()
    {
        var errors = _errors.Select(x => 
                new KeyValuePair<string, IEnumerable<ErrorElement>>(x.Key, x.Value.ToArray()))
            .ToImmutableDictionary();
        var infos = _additionalInformation.Select(x => 
                new KeyValuePair<string, string>(x.Key, x.Value))
            .ToImmutableDictionary();

        var errorVm = new ErrorVm(errors, infos);

        _errors = new Dictionary<string, List<ErrorElement>>();
        _additionalInformation = new Dictionary<string, string>();

        return errorVm;
    }

    public ErrorVmBuilder WithGlobalError(string message, string? data = null)
    {
        AddError(string.Empty, message, data);
        return this;
    }
    
    public ErrorVmBuilder WithError(string key, string message, string? data = null)
    {
        AddError(key, message, data);
        return this;
    }

    public ErrorVmBuilder WithInfo(string key, string message)
    {
        AddInfo(key, message);
        return this;
    }

    public ErrorVmBuilder WithInfo(string key, object message)
    {
        AddInfo(key, message.ToString());
        return this;
    }
    
    private void AddError(string key, string message, string? data = null)
    {
        if(_errors.TryGetValue(key, out var value))
        {
            value.Add(new ErrorElement(message, data));
        }
        else
        {
            _errors.Add(key, new List<ErrorElement> { new ErrorElement(message, data) });
        }
    }

    private void AddInfo(string key, string message)
    {
        _additionalInformation.Add(key, message);
    }
}

namespace WebApplication1.ViewModels;

public class ErrorVm
{
    public Dictionary<string, List<ErrorElement>> Errors { get; }

    public ErrorVm()
    {
        Errors = new Dictionary<string, List<ErrorElement>>();
    }

    public void AddError(string key, string message, string? data = null)
    {
        if(Errors.TryGetValue(key, out var value))
        {
            value.Add(new ErrorElement(message, data));
        }
        else
        {
            Errors.Add(key, new List<ErrorElement> { new ErrorElement(message, data) });
        }
    }
}

public record ErrorElement(
    string Message,
    string? Data
);

public class ErrorVmBuilder
{
    private ErrorVm _instance = new ErrorVm();
    
    public ErrorVm Build()
    {
        return _instance;
    }

    public ErrorVmBuilder WithGlobalError(string message, string? data = null)
    {
        _instance.AddError(string.Empty, message, data);
        return this;
    }
    
    public ErrorVmBuilder WithError(string key, string message, string? data = null)
    {
        _instance.AddError(key, message, data);
        return this;
    }
}
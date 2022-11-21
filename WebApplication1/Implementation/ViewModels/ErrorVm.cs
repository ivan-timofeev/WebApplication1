namespace WebApplication1.Implementation.ViewModels;

public class ErrorVm
{
    public Dictionary<string, List<string>> Errors { get; }

    public ErrorVm()
    {
        Errors = new Dictionary<string, List<string>>();
    }

    public void AddError(string key, string error)
    {
        if(Errors.TryGetValue(key, out var value))
        {
            value.Add(error);
        }
        else
        {
            Errors.Add(key, new List<string> { error });
        }
    }
}

public class ErrorVmBuilder
{
    private ErrorVm _instance;
    
    public ErrorVm Build()
    {
        return _instance ?? new ErrorVm();
    }

    public ErrorVmBuilder WithGlobalError(string error)
    {
        _instance.AddError(string.Empty, error);
        return this;
    }
    
    public ErrorVmBuilder WithError(string key, string error)
    {
        _instance.AddError(key, error);
        return this;
    }
}
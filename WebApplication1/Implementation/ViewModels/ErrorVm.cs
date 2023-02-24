using System.Collections.Immutable;

namespace WebApplication1.ViewModels;


public record ErrorVm
(
    IReadOnlyDictionary<string, IEnumerable<ErrorElement>> Errors,
    IReadOnlyDictionary<string, string> AdditionalInformation
);

public record ErrorElement
(
    string Message,
    string? Data
);

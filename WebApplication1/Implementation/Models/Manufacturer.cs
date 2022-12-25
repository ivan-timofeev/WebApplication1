using WebApplication1.Abstraction.Models;

namespace WebApplication1.Models;

public class Manufacturer : DomainModel
{
    public string? Name { get; set; }
    public string? ImageUri { get; set; }
}

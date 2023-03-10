#nullable disable
using WebApplication1.Abstraction.Models;

namespace WebApplication1.Models;

public class FileData : DomainModel
{
    public string FileUri { get; set; }
    public string FileName { get; set; }
}

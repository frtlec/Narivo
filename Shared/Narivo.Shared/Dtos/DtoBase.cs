using System.Text.Json.Serialization;

namespace Narivo.Shared.Dtos;
public class DtoBase
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    [JsonConstructor]
    public DtoBase()
    {
        
    }
}
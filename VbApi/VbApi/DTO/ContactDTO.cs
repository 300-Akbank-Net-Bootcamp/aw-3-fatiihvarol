using VbApi.DTO;
namespace VbApi.DTO;

public class ContactDTO
{
    public int CustomerId { get; set; } 
    public string ContactType { get; set; }
    public string Information { get; set; }
    public bool IsDefault { get; set; }
}

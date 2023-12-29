using VbApi.DTO;

namespace VbApi.DTO;

public class AccountDTO
{
    public int CustomerId { get; set; }
    public int AccountId { get; set; }
    public int AccountNumber { get; set; }
    public string IBAN { get; set; }
    public decimal Balance { get; set; }
    public string CurrencyType { get; set; }
    public string Name { get; set; }
    public DateTime OpenDate { get; set; }
    
    public List<EftTransactionDTO>? EftTransactionDtos { get; set; }

    
}

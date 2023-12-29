using VbApi.DTO;

namespace VbApi.DTO;

public class AccountTransactionDTO
{
    public int AccountId { get; set; }

    public string ReferenceNumber { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string TransferType { get; set; }

    
}

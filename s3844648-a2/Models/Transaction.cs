using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace s3844648_a2.Models;

public enum TransactionType
{
    Deposit = 1,
    Withdraw = 2,
    Transfer = 3,
    ServiceCharge = 4,
    BillPay = 5
}

public class Transaction
{
    [Display(Name = "ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionID { get; set; }

    [Display(Name = "Type")]
    public TransactionType TransactionType { get; set; }

    [Display(Name = "Account")]
    public int AccountID { get; set; }
    public virtual Account Account { get; set; }

    [Display(Name = "Destination Account")]
    public int? DestinationAccountID { get; set; }
    public virtual Account DestinationAccount { get; set; }

    [Column(TypeName = "money")]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }

    [StringLength(30, ErrorMessage = "Comment must be less than 30 characters")]
    public string? Comment { get; set; }

    [Display(Name = "Time")]
    public DateTime TransactionTimeUtc { get; set; }
}

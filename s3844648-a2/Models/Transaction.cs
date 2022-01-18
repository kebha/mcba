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
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int TransactionID { get; set; }

    [Required]
    public TransactionType TransactionType { get; set; }

    [Required]
    public int AccountID { get; set; }
    public virtual Account Account { get; set; }

    public int? DestinationAccountID { get; set; }
    public virtual Account DestinationAccount { get; set; }

    [Required]
    [Column(TypeName = "money")]
    [DataType(DataType.Currency)]
    [DisplayFormat(DataFormatString = "{0:C")]
    public decimal Amount { get; set; }

    [StringLength(30)]
    public string? Comment { get; set; }

    [Required]
    public DateTime TransactionTimeUtc { get; set; }
}

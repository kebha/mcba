using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace s3844648_a2.Models;

public enum AccountType
{
    Checking = 1,
    Saving = 2
}
public class Account
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "Account Number")]
    public int AccountID { get; set; }

    [Required]
    [Display(Name = "Type")]
    public AccountType AccountType { get; set; }

    public int CustomerID { get; set; }
    public virtual Customer Customer { get; set; }

    [Column(TypeName = "money")]
    [DataType(DataType.Currency)]
    [DisplayFormat(DataFormatString = "{0:C")]
    public decimal Balance { get; set; }

    [InverseProperty(nameof(Transaction.Account))]
    public virtual List<Transaction> Transactions { get; set; }
}

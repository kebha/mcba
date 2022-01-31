using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace s3844648_a2.Models;

public enum Period
{
    OneOff = 1,
    Monthly = 2
}

public class BillPay
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BillPayID { get; set; }

    public int AccountID { get; set; }
    public virtual Account Account { get; set; }

    public int PayeeID { get; set; }
    public virtual Payee Payee { get; set; }

    [Column(TypeName = "money")]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }

    [Display(Name = "Scheduled Time")]
    public DateTime ScheduleTimeUtc { get; set; }

    public Period Period { get; set; }

    public bool Blocked { get; set; } = false;
}
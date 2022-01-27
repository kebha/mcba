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

    [Display(Name = "Account")]
    public int AccountID { get; set; }
    public virtual Account Account { get; set; }

    public int PayeeID { get; set; }
    public virtual Payee Payee { get; set; }

    [Column(TypeName = "money")]
    [DataType(DataType.Currency)]
    public int Amount { get; set; }

    public DateTime ScheduleTimeUtc { get; set; }

    public Period Period { get; set; }
}
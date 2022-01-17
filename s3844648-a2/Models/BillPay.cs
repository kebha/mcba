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
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int BillPayID { get; set; }

    public int AccountID { get; set; }
    public virtual Account Account { get; set; }

    public int PayeeID { get; set; }
    public virtual Payee Payee { get; set; }

    [Required]
    [Column(TypeName = "money")]
    [DataType(DataType.Currency)]
    [DisplayFormat(DataFormatString = "{0:C")]
    public int Amount { get; set; }

    [Required]
    public DateTime ScheduleTimeUtc { get; set; }

    [Required]
    public Period Period { get; set; }
}
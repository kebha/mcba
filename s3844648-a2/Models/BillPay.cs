namespace s3844648_a2.Models;

public class BillPay
{

    public int BillPayID { get; set; }


    public int AccountNumber { get; set; }


    public int PayeeID { get; set; }

    public int Amount { get; set; }


    public DateTime ScheduleTimeUtc { get; set; }


    public string Period { get; set; }
}
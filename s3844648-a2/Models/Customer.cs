

namespace s3844648_a2.Models;

public class Customer
{

    public int CustomerID { get; set; }


    public string Name { get; set; }


    public string TFN { get; set; }


    public string Address { get; set; }


    public string Suburb { get; set; }


    public string State { get; set; }


    public string PostCode { get; set; }


    public string Mobile { get; set; }


    public List<Account> Accounts { get; set; }


    public Login Login { get; set; }
}

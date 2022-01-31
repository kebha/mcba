using AdminPortalWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace AdminPortalWeb.Models.DataManager;

public class CustomerManager 
{
    private readonly MyContext _context;

    public CustomerManager(MyContext context)
    {
        _context = context;
    }

    //Customer
    public IEnumerable<Customer> GetCustomers()
    {
        return _context.Customers.Include(x => x.Login).ToList();
    }
    public IEnumerable<Customer> GetCustomer(int id)
    {
        return _context.Customers.Where(x => x.CustomerID == id).ToList();
    }
    public void UpdateCustomer(int id, CustomerDto input)
    {
        var customer = _context.Customers.Find(id);
        customer.TFN = input.TFN;
        customer.Address = input.Address;
        customer.Suburb = input.Suburb;
        customer.State = input.State;
        customer.Postcode = input.Postcode;
        customer.Mobile = input.Mobile;
        _context.SaveChanges();
    }

    //Login
    public void UpdateLogin(string id, bool status)
    {
        var login = _context.Logins.Find(id);
        login.Locked = status;
        _context.SaveChanges();
    }

    //Account
    public IEnumerable<Account> GetAccounts()
    {
        return _context.Accounts.ToList();
    }

    //Transaction
    public IEnumerable<Transaction> GetTransactions(int id)
    {
        return _context.Transactions.Where(x => x.AccountID == id).ToList();
    }

    //BillPay
    public IEnumerable<BillPay> GetBillPays()
    {
        return _context.BillPays.ToList();
    }

    public void UpdateBillPay(int id, bool status)
    {
        var billPay = _context.BillPays.Find(id);
        billPay.Blocked = status;
        _context.SaveChanges();
    }
}

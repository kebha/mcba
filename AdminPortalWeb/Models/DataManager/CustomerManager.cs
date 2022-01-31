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

    public IEnumerable<Customer> GetCustomers()
    {
        return _context.Customers.Include(x => x.Login).ToList();
    }

    public IEnumerable<Account> GetAccounts()
    {
        return _context.Accounts.ToList();
    }

    public IEnumerable<Transaction> GetTransactions(int id)
    {
        return _context.Transactions.Where(x => x.AccountID == id).ToList();
    }
}

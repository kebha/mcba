using AdminPortalWeb.Data;

namespace AdminPortalWeb.Models.DataManager;

public class CustomerManager 
{
    private readonly MyContext _context;

    public CustomerManager(MyContext context)
    {
        _context = context;
    }

    public IEnumerable<Account> GetAll()
    {
        return _context.Accounts.ToList();
    }

    public IEnumerable<Transaction> GetTransactions(int id)
    {
        return _context.Transactions.Where(x => x.AccountID == id).ToList();
    }
}

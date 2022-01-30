using AdminPortalWeb.Data;

namespace AdminPortalWeb.Models.DataManager;

public class AdminManager 
{
    private readonly MyContext _context;

    public AdminManager(MyContext context)
    {
        _context = context;
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

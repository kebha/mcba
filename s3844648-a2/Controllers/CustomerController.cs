using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using s3844648_a2.Data;
using s3844648_a2.Utilities;
using s3844648_a2.Filters;
using s3844648_a2.Models;

namespace s3844648_a2.Controllers;

//this class is built off of code from day 6 McbaExampleWithLogin
[AuthorizeCustomer]
public class CustomerController : Controller
{
    private readonly MyContext _context;

    private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

    public CustomerController(MyContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var customer = await _context.Customers.Include(x => x.Accounts).
            FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

        return View(customer);
    }

    public async Task<IActionResult> Deposit(int id)
    {

        View(await _context.Accounts.Include(x => x.Transactions).
                FirstOrDefaultAsync(x => x.AccountID == id));
    }
} 

    [HttpPost]
    public async Task<IActionResult> Deposit(int id, decimal amount, string comment = null)
    {
        var account = await _context.Accounts.Include(x => x.Transactions).
            FirstOrDefaultAsync(x => x.AccountID == id);

        if (amount <= 0)
            ModelState.AddModelError(nameof(amount), "Amount must be positive.");
        if(amount.HasMoreThanTwoDecimalPlaces())
            ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
        if(!ModelState.IsValid)
        {
            ViewBag.Amount = amount;
            return View(account);
        }

        account.Balance += amount;
        account.Transactions.Add(new Transaction
        {
            AccountID = account.AccountID,
            TransactionType = TransactionType.Deposit,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.UtcNow
        });
        
        /*_context.Transactions.Add(new Transaction
        {
            AccountID = account.AccountID,
            TransactionType = TransactionType.Deposit,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.UtcNow
        });*/

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}

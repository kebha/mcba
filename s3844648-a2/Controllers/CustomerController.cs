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
        var customer = await _context.Customers.FindAsync(CustomerID);

        return View(customer);
    }

    public IActionResult Deposit(int id)
    {
        return View(new Transaction() {AccountID = id});
    }

    [HttpPost]
    public async Task<IActionResult> Deposit(int id, Transaction transaction)
    {
        if (transaction.Amount <= 0)
            ModelState.AddModelError(nameof(transaction.Amount), "Amount must be positive.");
        if (transaction.Amount.HasMoreThanTwoDecimalPlaces())
            ModelState.AddModelError(nameof(transaction.Amount), "Amount cannot have more than 2 decimal places.");
        if (!ModelState.IsValid)
        {
            return View(transaction);
        }

        return RedirectToAction(nameof(Confirmation), transaction);
    }

    public IActionResult Confirmation(Transaction transaction) => View(transaction);

    [HttpPost]
    public async Task<IActionResult> Confirmation(Transaction transaction, int i=0)
    {
        var account = await _context.Accounts.FindAsync(transaction.AccountID);

        account.Balance += transaction.Amount;
        account.Transactions.Add(new Transaction
        {
            TransactionType = transaction.TransactionType,
            DestinationAccountID = transaction.DestinationAccountID,
            Amount = transaction.Amount,
            Comment = transaction.Comment,
            TransactionTimeUtc = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}

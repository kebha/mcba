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

    private readonly decimal _withdrawFee = 0.05m;

    private readonly decimal _transferFee = 0.10m;

    public CustomerController(MyContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var customer = await _context.Customers.FindAsync(CustomerID);

        return View(customer);
    }

    public IActionResult Deposit(int id) => View(new Transaction() {AccountID = id});

    [HttpPost]
    public async Task<IActionResult> Deposit(int id, decimal amount, string comment)
    {
        if (amount <= 0)
            ModelState.AddModelError(nameof(amount), "Amount must be positive.");
        if (amount.HasMoreThanTwoDecimalPlaces())
            ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
        if (!ModelState.IsValid)
        {
            return View(new Transaction() { AccountID = id });
        }

        var transaction = new Transaction()
        {
            TransactionType = TransactionType.Deposit,
            AccountID = id,
            DestinationAccountID = null,
            Amount = amount,
            Comment = comment
        };

        return RedirectToAction(nameof(Confirmation), transaction);
    }

    public IActionResult Withdraw(int id) => View(new Transaction() { AccountID = id });

    [HttpPost]
    public async Task<IActionResult> Withdraw(int id, Transaction transaction)
    {
        var account = await _context.Accounts.FindAsync(id);
        var availableBalance = account.AccountType == AccountType.Savings ? account.Balance : account.Balance - 300;
        var serviceCharge = account.Transactions.Count >= 2 ? _withdrawFee : 0;

        if (transaction.Amount <= 0)
            ModelState.AddModelError(nameof(transaction.Amount), "Amount must be positive.");
        if (transaction.Amount.HasMoreThanTwoDecimalPlaces())
            ModelState.AddModelError(nameof(transaction.Amount), "Amount cannot have more than 2 decimal places.");
        if (transaction.Amount + serviceCharge > availableBalance)
            ModelState.AddModelError(nameof(transaction.Amount), "This transaction would put you below your minimum balance");
        if (!ModelState.IsValid)
        {
            return View(transaction);
        }

        return RedirectToAction(nameof(Confirmation), transaction);
    }

    public IActionResult Transfer(int id) => View(new Transaction() { AccountID = id });

    [HttpPost]
    public async Task<IActionResult> Transfer(int id, Transaction transaction)
    {
        var account = await _context.Accounts.FindAsync(id);
        var availableBalance = account.AccountType == AccountType.Savings ? account.Balance : account.Balance - 300;
        var serviceCharge = account.Transactions.Count >= 2 ? _transferFee : 0;

        if (transaction.Amount <= 0)
            ModelState.AddModelError(nameof(transaction.Amount), "Amount must be positive.");
        if (transaction.Amount.HasMoreThanTwoDecimalPlaces())
            ModelState.AddModelError(nameof(transaction.Amount), "Amount cannot have more than 2 decimal places.");
        if (transaction.Amount + serviceCharge > availableBalance)
            ModelState.AddModelError(nameof(transaction.Amount), "This transaction would put you below your minimum balance");
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

    public async Task<bool> ServiceChargeApplies(int accountID)
    {
        var account = await _context.Accounts.FindAsync(accountID);
        
        var withdrawsAndTransfers = new List<Transaction>();

        foreach (var transaction in account.Transactions)
        {
            
        }

        return true;
    }
}

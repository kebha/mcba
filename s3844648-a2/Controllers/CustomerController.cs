using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using s3844648_a2.Data;
using s3844648_a2.Utilities;
using s3844648_a2.Filters;
using s3844648_a2.Models;
using X.PagedList;

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

    public async Task<IActionResult> MyStatements(int id, int? page = 1, int pageSize = 4)
    {
        ViewBag.Account = await _context.Accounts.FindAsync(id);

        //Page the orders
        var pagedList = await _context.Transactions.Where(x => x.AccountID == id).
            OrderBy(x => x.TransactionTimeUtc).ToPagedListAsync(page, pageSize);

        return View(pagedList);
    }

    public IActionResult Deposit (int id) => View(new Transaction() { AccountID = id });
    public IActionResult Withdraw(int id) => View(new Transaction() { AccountID = id });
    public IActionResult Transfer(int id) => View(new Transaction() { AccountID = id });

    [HttpPost]
    public async Task<IActionResult> Deposit(int id, decimal amount, string? comment)
    {
        //validation
        if (amount <= 0)
            ModelState.AddModelError(nameof(amount), "Amount must be positive.");
        if (amount.HasMoreThanTwoDecimalPlaces())
            ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
        if (!ModelState.IsValid)
            return View(new Transaction() { AccountID = id });

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

    [HttpPost]
    public async Task<IActionResult> Withdraw(int id, decimal amount, string? comment)
    {
        //validation
        var account = await _context.Accounts.FindAsync(id);
        var availableBalance = account.AccountType == AccountType.Savings ? account.Balance : account.Balance - 300;
        var serviceCharge = await ServiceChargeApplies(id) ? _withdrawFee : 0;
        if (amount <= 0)
            ModelState.AddModelError(nameof(amount), "Amount must be positive.");
        if (amount.HasMoreThanTwoDecimalPlaces())
            ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
        if (amount + serviceCharge > availableBalance)
            ModelState.AddModelError(nameof(amount), "This transaction would put you below your minimum balance.");
        if (!ModelState.IsValid)
            return View(new Transaction() { AccountID = id });

        var transaction = new Transaction()
        {
            TransactionType = TransactionType.Withdraw,
            AccountID = id,
            DestinationAccountID = null,
            Amount = amount,
            Comment = comment
        };
        return RedirectToAction(nameof(Confirmation), transaction);
    }

    [HttpPost]
    public async Task<IActionResult> Transfer(int id, int destinationAccountID, decimal amount, string? comment)
    {
        //validation
        var account = await _context.Accounts.FindAsync(id);
        var availableBalance = account.AccountType == AccountType.Savings ? account.Balance : account.Balance - 300;
        var serviceCharge = await ServiceChargeApplies(id) ? _transferFee : 0;
        if (amount <= 0)
            ModelState.AddModelError(nameof(amount), "Amount must be positive.");
        if (amount.HasMoreThanTwoDecimalPlaces())
            ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
        if (amount + serviceCharge > availableBalance)
            ModelState.AddModelError(nameof(amount), "This transaction would put you below your minimum balance.");
        if (!_context.Accounts.Any(x => x.AccountID == destinationAccountID))
            ModelState.AddModelError(nameof(destinationAccountID), "This account does not exist.");
        if (destinationAccountID == id)
            ModelState.AddModelError(nameof(destinationAccountID), "Cannot transfer to same account.");
        if (!ModelState.IsValid)
            return View(new Transaction() { AccountID = id });

        var transaction = new Transaction()
        {
            TransactionType = TransactionType.Transfer,
            AccountID = id,
            DestinationAccountID = destinationAccountID,
            Amount = amount,
            Comment = comment
        };
        return RedirectToAction(nameof(Confirmation), transaction);
    }

    public IActionResult Confirmation(Transaction transaction) => View(transaction);

    [HttpPost]
    public async Task<IActionResult> Confirmation(Transaction transaction, int i=0)
    {
        //transaction
        var account = await _context.Accounts.FindAsync(transaction.AccountID);
        account.Balance += transaction.TransactionType == TransactionType.Deposit ? transaction.Amount : transaction.Amount * -1;
        account.Transactions.Add(new Transaction
        {
            TransactionType = transaction.TransactionType,
            DestinationAccountID = transaction.DestinationAccountID,
            Amount = transaction.Amount,
            Comment = transaction.Comment,
            TransactionTimeUtc = DateTime.UtcNow
        });
        if (transaction.TransactionType == TransactionType.Transfer)
        {
            var destAccount = await _context.Accounts.FindAsync(transaction.DestinationAccountID);
            destAccount.Balance += transaction.Amount;
            destAccount.Transactions.Add(new Transaction()
            {
                TransactionType = TransactionType.Transfer,
                DestinationAccountID = null,
                Amount = transaction.Amount,
                Comment = transaction.Comment,
                TransactionTimeUtc = DateTime.UtcNow
            });
        }

        //service charge
        if (await ServiceChargeApplies(transaction.AccountID) && (transaction.TransactionType == TransactionType.Withdraw || transaction.TransactionType == TransactionType.Transfer))
        {
            account.Balance += transaction.TransactionType == TransactionType.Withdraw ? _withdrawFee * -1 : _transferFee * -1;
            account.Transactions.Add(new Transaction
            {
                TransactionType = TransactionType.ServiceCharge,
                DestinationAccountID = null,
                Amount = transaction.TransactionType == TransactionType.Withdraw ? _withdrawFee : _transferFee,
                Comment = transaction.Comment,
                TransactionTimeUtc = DateTime.UtcNow
            });
        }
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<bool> ServiceChargeApplies(int accountID)
    {
        var account = await _context.Accounts.FindAsync(accountID);
        var withdrawsAndTransfers = new List<Transaction>();

        foreach (var transaction in account.Transactions)
        {
            if (transaction.TransactionType == TransactionType.Withdraw || (transaction.TransactionType == TransactionType.Transfer && transaction.DestinationAccountID != null))
                withdrawsAndTransfers.Add(transaction);
        }

        return withdrawsAndTransfers.Count >= 2;
    }
}

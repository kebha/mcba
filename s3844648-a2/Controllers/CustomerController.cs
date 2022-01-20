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

    private int _selectedAccount;

    private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

    public CustomerController(MyContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var customer = await _context.Customers.Include(x => x.Accounts).
            FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

        return View(customer);
    }

    public IActionResult Deposit(int id)
    {
        _selectedAccount = id;
        return View(new Transaction() {AccountID = id});
    }

    [HttpPost]
    public async Task<IActionResult> Deposit(int id, Transaction input)
    {
        // Only check validation for the relevant field of this step instead of all fields.
        //if (ModelState.GetValidationState(nameof(input.FirstName)) != ModelValidationState.Valid)
        //    return View(input);

        var account = await _context.Accounts.Include(x => x.Transactions).
            FirstOrDefaultAsync(x => x.AccountID == id);

        account.Balance += input.Amount;
        account.Transactions.Add(new Transaction
        {
            TransactionType = TransactionType.Deposit,
            Amount = input.Amount,
            Comment = input.Comment,
            TransactionTimeUtc = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}

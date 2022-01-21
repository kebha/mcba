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

    public IActionResult Deposit(int id) => View(new Transaction() {AccountID = id});

    //public IActionResult Confirm(int id) => View(new Transaction() { AccountID = id });

    [HttpPost]
    public async Task<IActionResult> Deposit(int id, Transaction transaction)
    {
        var account = await _context.Accounts.FindAsync(id);

        account.Balance += transaction.Amount;
        account.Transactions.Add(new Transaction
        {
            TransactionType = TransactionType.Deposit,
            Amount = transaction.Amount,
            Comment = transaction.Comment,
            TransactionTimeUtc = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


}

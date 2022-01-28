using Microsoft.AspNetCore.Mvc;
using s3844648_a2.Data;
using s3844648_a2.Filters;
using s3844648_a2.Models;
using s3844648_a2.Utilities;

namespace s3844648_a2.Controllers;

[AuthorizeCustomer]
public class BillPayController : Controller
{
    private readonly MyContext _context;

    private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

    public BillPayController(MyContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Customers.FindAsync(CustomerID));

    public async Task<IActionResult> New() => View();

    [HttpPost]
    public async Task<IActionResult> New(BillPay billPay)
    {
        //validation
        ModelState.Remove("Account");
        ModelState.Remove("Payee");
        var customer = await _context.Customers.FindAsync(CustomerID);
        bool isUsersAccount = false;
        foreach (var account in customer.Accounts)
        {
            if (billPay.AccountID == account.AccountID)
                isUsersAccount = true;
        }

        if (billPay.Amount <= 0)
            ModelState.AddModelError(nameof(billPay.Amount), "Amount must be positive.");
        if (billPay.Amount.HasMoreThanTwoDecimalPlaces())
            ModelState.AddModelError(nameof(billPay.Amount), "Amount cannot have more than 2 decimal places.");
        if (!_context.Accounts.Any(x => x.AccountID == billPay.AccountID))
            ModelState.AddModelError(nameof(billPay.AccountID), "This account does not exist.");
        if (!_context.Payees.Any(x => x.PayeeID == billPay.PayeeID))
            ModelState.AddModelError(nameof(billPay.PayeeID), "This Payee does not exist.");
        if (billPay.ScheduleTimeUtc < DateTime.Now)
            ModelState.AddModelError(nameof(billPay.ScheduleTimeUtc), "Scheduled date must be in the future");
        if (!isUsersAccount)
            ModelState.AddModelError(nameof(billPay.AccountID), "May only withdraw funds from your accounts");
        if (!ModelState.IsValid)
            return View();

        //insert BillPay
        _context.BillPays.Add(new BillPay
        {
            AccountID = billPay.AccountID,
            PayeeID = billPay.PayeeID,
            Amount = billPay.Amount,
            ScheduleTimeUtc = billPay.ScheduleTimeUtc.ToUniversalTime(),
            Period = billPay.Period
        });
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index), await _context.Customers.FindAsync(CustomerID));
    }

    public async Task<IActionResult> Modify() => View();

    public async Task<IActionResult> Cancel() => View();
}

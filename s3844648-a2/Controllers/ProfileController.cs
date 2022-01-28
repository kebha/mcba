using Microsoft.AspNetCore.Mvc;
using s3844648_a2.Data;
using s3844648_a2.Filters;
using s3844648_a2.Models;
using SimpleHashing;

namespace s3844648_a2.Controllers;

[AuthorizeCustomer]
public class ProfileController : Controller
{
    private readonly MyContext _context;

    private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

    public ProfileController(MyContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Customers.FindAsync(CustomerID));

    [HttpPost]
    public async Task<IActionResult> Index(Customer input)
    {
        //Validation
        ModelState.Remove("Login");
        ModelState.Remove("Accounts");
        if (!ModelState.IsValid)
            return View(await _context.Customers.FindAsync(CustomerID));

        //Update changes
        var customer = await _context.Customers.FindAsync(CustomerID);
        customer.Name = input.Name;
        customer.TFN = input.TFN;
        customer.Address = input.Address;
        customer.Suburb = input.Suburb;
        customer.State = input.State;
        customer.Postcode = input.Postcode;
        customer.Mobile = input.Mobile;
        await _context.SaveChangesAsync();

        return View(await _context.Customers.FindAsync(CustomerID)); ;
    }

    public async Task<IActionResult> ChangePassword() => View();

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel input)
    {
        //Validation
        if (!ModelState.IsValid)
            return View();

        //Update changes
        var customer = await _context.Customers.FindAsync(CustomerID);
        customer.Login.PasswordHash = PBKDF2.Hash(input.NewPassword);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index), await _context.Customers.FindAsync(CustomerID));
    }
}

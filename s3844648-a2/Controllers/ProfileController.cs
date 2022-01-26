using Microsoft.AspNetCore.Mvc;
using s3844648_a2.Data;
using s3844648_a2.Filters;
using s3844648_a2.Models;

namespace s3844648_a2.Controllers;

[AuthorizeCustomer]
public class ProfileController : Controller
{
    private readonly MyContext _context;

    private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

    public ProfileController(MyContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Customers.FindAsync(CustomerID));

    [HttpPost]
    public async Task<IActionResult> Index(string name, string? tfn, string? address, string? suburb, State? state, int? postCode, string? mobile)
    {
        //Validation
        if (!ModelState.IsValid)
            return View(await _context.Customers.FindAsync(CustomerID)); ;

        //Update changes
        var customer = await _context.Customers.FindAsync(CustomerID);
        customer.Name = name;
        customer.TFN = tfn;
        customer.Address = address;
        customer.Suburb = suburb;
        customer.State = state;
        customer.Postcode = postCode;
        customer.Mobile = mobile;
        await _context.SaveChangesAsync();

        return View(await _context.Customers.FindAsync(CustomerID)); ;
    }

    public async Task<IActionResult> Password() => View(await _context.Customers.FindAsync(CustomerID));

    [HttpPost]
    public async Task<IActionResult> Password(string password)
    {
        //Validation
        if (!ModelState.IsValid)
            return View(); ;

        //Hash


        //Update changes
        var customer = await _context.Customers.FindAsync(CustomerID);
        customer.Login.PasswordHash = password;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index), await _context.Customers.FindAsync(CustomerID));
    }
}

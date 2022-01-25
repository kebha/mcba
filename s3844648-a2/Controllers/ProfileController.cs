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

    public async Task<IActionResult> Index()
    {
        var customer = await _context.Customers.FindAsync(CustomerID);

        return View(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Index(Customer customer)
    {
        //Validation

        return View(customer);
    }
}

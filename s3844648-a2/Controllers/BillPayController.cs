using Microsoft.AspNetCore.Mvc;
using s3844648_a2.Data;
using s3844648_a2.Filters;
using s3844648_a2.Models;

namespace s3844648_a2.Controllers;

[AuthorizeCustomer]
public class BillPayController : Controller
{
    private readonly MyContext _context;

    private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

    public BillPayController(MyContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Customers.FindAsync(CustomerID));

    public async Task<IActionResult> Modify() => View();

    public async Task<IActionResult> Cancel() => View();
}

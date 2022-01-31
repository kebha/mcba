using Microsoft.AspNetCore.Mvc;
using s3844648_a2.Data;
using s3844648_a2.Models;
using s3844648_a2.Data;
using SimpleHashing;

namespace s3844648_a2.Controllers;

//this class is built off of code from day 6 McbaExampleWithLogin

public class LoginController : Controller
{
    private readonly MyContext _context;

    public LoginController(MyContext context) => _context = context;

    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Index(string loginID, string password)
    {
        var login = await _context.Logins.FindAsync(loginID);

        if (login == null || string.IsNullOrEmpty(password) || !PBKDF2.Verify(login.PasswordHash, password))
        { 
            ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
            return View(new Login { LoginID = loginID });
        }
        if (login.Locked)
        {
            ModelState.AddModelError("LoginFailed", "Your Account has been Locked. You may not currently Login");
            return View(new Login { LoginID = loginID });
        }

        // Login customer.
        HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
        HttpContext.Session.SetString(nameof(Customer.Name), login.Customer.Name);

        return RedirectToAction("Index", "Account");
    }

    public IActionResult Logout()
    {
        // Logout customer.
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Login");
    }
}

using Microsoft.AspNetCore.Mvc;
using AdminPortalWeb.Data;
using AdminPortalWeb.Models;
using AdminPortalWeb.Data;

namespace AdminPortalWeb.Controllers;

//this class is built off of code from day 6 McbaExampleWithLogin

public class LoginController : Controller
{
    private readonly MyContext _context;

    public LoginController(MyContext context) => _context = context;

    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Index(string loginID, string password)
    {

        if (!loginID.Equals("admin") || !password.Equals("admin"))
        { 
            ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
            return View(new Login { LoginID = loginID });
        }

        // Login admin.
        //HttpContext.Session.SetString(nameof(Customer.CustomerID), "admin");
        //HttpContext.Session.SetString(nameof(Customer.Name), "admin");

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        // Logout admin.
        //HttpContext.Session.Clear();

        return RedirectToAction("Index", "Login");
    }
}

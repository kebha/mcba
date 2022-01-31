using AdminPortalWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AdminPortalWeb.Controllers;

public class ProfileController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private HttpClient Client => _clientFactory.CreateClient();

    public ProfileController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

    public async Task<IActionResult> Index()
    {
        // Get Customers
        var response = await Client.GetStringAsync("api/customers");
        var customers = JsonConvert.DeserializeObject<List<Customer>>(response);

        return View(customers);
    }

    public async Task<IActionResult> Modify()
    {
        return View();
    }

    public async Task<IActionResult> Lock()
    {
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> UnLock()
    {
        return RedirectToAction(nameof(Index));
    }
}

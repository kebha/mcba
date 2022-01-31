using System.Text;
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

    public async Task<IActionResult> Modify(int id)
    {
        // Get Customer
        var response = await Client.GetStringAsync($"api/customers/{id}");
        var customers = JsonConvert.DeserializeObject<List<CustomerDto>>(response);

        return View(customers[0]);
    }

    [HttpPost]
    public async Task<IActionResult> Modify(CustomerDto input)
    {
        //Validation
        ModelState.Remove("Login");
        ModelState.Remove("Accounts");
        if (!ModelState.IsValid)
        {
            var response = await Client.GetStringAsync($"api/customers/{input.CustomerID}");
            var customers = JsonConvert.DeserializeObject<List<CustomerDto>>(response);
            return View(customers[0]);
        }


        //Update changes
        var content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
        await Client.PutAsync($"api/customers", content);

        return RedirectToAction(nameof(Index));
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

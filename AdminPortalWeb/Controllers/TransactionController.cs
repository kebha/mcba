using System.Text;
using Microsoft.AspNetCore.Mvc;
using AdminPortalWeb.Models;
using Newtonsoft.Json;

namespace AdminPortalWeb.Controllers;

public class TransactionController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private HttpClient Client => _clientFactory.CreateClient();

    public TransactionController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

    public async Task<IActionResult> Index()
    {
        var response = await Client.GetAsync("api/accounts");

        if (!response.IsSuccessStatusCode)
            throw new Exception();

        // Storing the response details received from web api.
        var result = await response.Content.ReadAsStringAsync();

        // Deserializing the response received from web api and storing into a list.
        var accounts = JsonConvert.DeserializeObject<List<Account>>(result);

        return View(accounts);
    }

    public async Task<IActionResult> SelectPeriod(int id)
    {
        return View();
    }

    /*[HttpPost]
    public async Task<IActionResult> SelectPeriod(int id, D)
    {
        var response = await Client.GetStringAsync("api/accounts");

        // Deserializing the response received from web api and storing into a list.
        var accounts = JsonConvert.DeserializeObject<List<Account>>(response);

        return View(accounts);
    }*/
}

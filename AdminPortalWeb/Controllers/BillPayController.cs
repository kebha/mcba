using AdminPortalWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AdminPortalWeb.Controllers;

public class BillPayController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private HttpClient Client => _clientFactory.CreateClient();

    public BillPayController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

    public async Task<IActionResult> Index()
    {
        // Get BillPays
        var response = await Client.GetStringAsync("api/billpays");
        var billPays = JsonConvert.DeserializeObject<List<BillPay>>(response);

        return View(billPays);
    }

    public async Task<IActionResult> Block(int id)
    {
        //Set blocked true
        await Client.PutAsync($"api/billpays/{id}/{true}", null);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> UnBlock(int id)
    {
        //Set blocked true
        await Client.PutAsync($"api/billpays/{id}/{false}", null);
        return RedirectToAction(nameof(Index));
    }
}

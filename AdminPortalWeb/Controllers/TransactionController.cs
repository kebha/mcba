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
        //get accounts
        var response = await Client.GetStringAsync("api/accounts");
        var accounts = JsonConvert.DeserializeObject<List<Account>>(response);

        return View(accounts);
    }

    public async Task<IActionResult> SelectPeriod(int id) => View(new SelectPeriodModel {AccountID = id});

    [HttpPost]
    public async Task<IActionResult> SelectPeriod(SelectPeriodModel period)
    {
        return RedirectToAction(nameof(History), period);
    }

    public async Task<IActionResult> History(SelectPeriodModel period)
    {
        //get transactions of selected account
        var response = await Client.GetStringAsync($"api/transactions/{period.AccountID}");
        var transactions = JsonConvert.DeserializeObject<List<Transaction>>(response);

        //convert time to local time
        foreach (var transaction in transactions)
            transaction.TransactionTimeUtc = transaction.TransactionTimeUtc.ToLocalTime();

        //filter period
        if (period.StartDate != null)
            transactions.RemoveAll(x => x.TransactionTimeUtc < period.StartDate);
        if (period.EndDate != null)
            transactions.RemoveAll(x => x.TransactionTimeUtc > period.EndDate);

        return View(transactions);
    } 
}

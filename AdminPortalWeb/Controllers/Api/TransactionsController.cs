using Microsoft.AspNetCore.Mvc;
using AdminPortalWeb.Models;
using AdminPortalWeb.Models.DataManager;

namespace AdminPortalWeb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly CustomerManager _repo;

    public TransactionsController(CustomerManager repo)
    {
        _repo = repo;
    }

    // GET api/transactions/1
    // Gets all Transactions from the specified account
    [HttpGet("{id}")]
    public IEnumerable<Transaction> Get(int id)
    {
        return _repo.GetTransactions(id);
    }
}

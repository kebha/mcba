using Microsoft.AspNetCore.Mvc;
using AdminPortalWeb.Models;
using AdminPortalWeb.Models.DataManager;

namespace AdminPortalWeb.Api.Controllers;

// This class was built off of code from Lectorial 9
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly CustomerManager _repo;

    public AdminController(CustomerManager repo)
    {
        _repo = repo;
    }

    // GET api/transactions/1
    [HttpGet("{id}")]
    public IEnumerable<Transaction> Get(int id)
    {
        return _repo.GetTransactions(id);
    }
}

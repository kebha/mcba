using Microsoft.AspNetCore.Mvc;
using AdminPortalWeb.Models;
using AdminPortalWeb.Models.DataManager;

namespace AdminPortalWeb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly CustomerManager _repo;

    public AccountsController(CustomerManager repo)
    {
        _repo = repo;
    }

    // GET: api/accounts
    // Gets all accounts
    [HttpGet]
    public IEnumerable<Account> Get()
    {
        return _repo.GetAll();
    }
}

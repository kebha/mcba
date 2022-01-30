using Microsoft.AspNetCore.Mvc;
using AdminPortalWeb.Models;
using AdminPortalWeb.Models.DataManager;

namespace AdminPortalWeb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly AdminManager _repo;

    public AccountsController(AdminManager repo)
    {
        _repo = repo;
    }

    // GET: api/accounts
    [HttpGet]
    public IEnumerable<Account> Get()
    {
        return _repo.GetAccounts();
    }
}

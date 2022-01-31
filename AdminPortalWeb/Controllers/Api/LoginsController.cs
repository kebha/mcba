using Microsoft.AspNetCore.Mvc;
using AdminPortalWeb.Models;
using AdminPortalWeb.Models.DataManager;
using Newtonsoft.Json;

namespace AdminPortalWeb.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class LoginsController : Controller
{
    private readonly CustomerManager _repo;

    public LoginsController(CustomerManager repo)
    {
        _repo = repo;
    }

    // PUT api/logins/1/true
    // Locks/unlocks specified login
    [HttpPut("{id}/{status}")]
    public void Put(string id, bool status)
    {
        _repo.UpdateLogin(id, status);
    }
}

using Microsoft.AspNetCore.Mvc;
using AdminPortalWeb.Models;
using AdminPortalWeb.Models.DataManager;
using Newtonsoft.Json;

namespace AdminPortalWeb.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class BillPaysController : Controller
{
    private readonly CustomerManager _repo;

    public BillPaysController(CustomerManager repo)
    {
        _repo = repo;
    }

    // GET api/billpays
    // Gets all billPays
    [HttpGet]
    public IEnumerable<BillPay> Get()
    {
        return _repo.GetBillPays();
    }

    // PUT api/billpays/1/true
    // blocks/unblocks specified BillPay
    [HttpPut("{id}/{status}")]
    public void Put(int id, bool status)
    {
        _repo.UpdateBillPay(id, status);
    }
}
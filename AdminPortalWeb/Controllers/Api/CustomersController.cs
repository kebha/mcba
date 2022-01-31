using Microsoft.AspNetCore.Mvc;
using AdminPortalWeb.Models;
using AdminPortalWeb.Models.DataManager;
using Newtonsoft.Json;

namespace AdminPortalWeb.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : Controller
{
    private readonly CustomerManager _repo;

    public CustomersController(CustomerManager repo)
    {
        _repo = repo;
    }

    // GET api/customers
    // Gets all customers with login
    [HttpGet]
    public IEnumerable<Customer> Get()
    {
        return _repo.GetCustomers();
    }

    // GET api/customers/1
    // Gets customer info for specified customer
    [HttpGet("{id}")]
    public IEnumerable<Customer> Get(int id)
    {
        return _repo.GetCustomer(id);
    }

    // PUT api/customers
    // Modifies customer data
    [HttpPut]
    public void Put([FromBody] CustomerDto customer)
    {
        _repo.UpdateCustomer(customer.CustomerID, customer);
    }
}

using Application.Interfaces;
using Domain.Customers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        return Ok(_customerService.GetAllCustomers());
    }

    [HttpPost]
    public async Task<ActionResult> Create(List<Customer> customers)
    {
        await _customerService.InsertCustomers(customers);
        return Created();
    }
}

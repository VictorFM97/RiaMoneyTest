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
    [ProducesResponseType(200, Type = typeof(List<Customer>))]
    [ProducesResponseType(500, Type = typeof(List<string>))]
    public ActionResult Get()
    {
        return Ok(_customerService.GetAllCustomers());
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(422, Type = typeof(List<string>))]
    [ProducesResponseType(500, Type = typeof(List<string>))]
    public ActionResult Create(List<Customer> customers)
    {
        var errors = _customerService.InsertCustomers(customers);

        if (errors.Any())
        {
            return UnprocessableEntity(errors);
        }

        return Ok();
    }
}

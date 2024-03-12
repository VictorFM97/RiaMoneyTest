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
    public ActionResult Get()
    {
        return Ok(_customerService.GetAllCustomers());
    }

    [HttpPost]
    public ActionResult Create(List<Customer> customers)
    {
        var errors = _customerService.InsertCustomers(customers);

        if (errors.Any())
        {
            return UnprocessableEntity(errors);
        }

        return Created();
    }
}

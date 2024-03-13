using API.Controllers;
using Application.Interfaces;
using Domain.Customers;
using Microsoft.AspNetCore.Mvc;

namespace Tests.API.Controllers;
public class CustomersControllerTest
{
    private readonly Mock<ICustomerService> _customerService;
    private CustomersController _customerController;

    public CustomersControllerTest()
        : base()
    {
        _customerService = new Mock<ICustomerService>();
    }

    [Fact]
    public void GetAll_ShouldThrowException()
    {
        var customers = new List<Customer>
            {
                new Customer(1, "name", "name", 18),
                new Customer(2, "name2", "name2", 19),
                new Customer(3, "name3", "name3", 20),
            };

        _customerService.Setup(x => x.GetAllCustomers())
            .Returns(customers);

        _customerController = new CustomersController(_customerService.Object);
        var result = _customerController.Get();

        result.Should().BeOfType<OkObjectResult>();

        var objectResult = result as OkObjectResult;
        objectResult.Value.Should().BeOfType<List<Customer>>();

        var customerList = objectResult.Value as List<Customer>;
        customerList.Should().NotBeNull().And.BeEquivalentTo(customers);
    }

    [Fact]
    public void Create_ShouldReturnUnprocessableEntityIfThereAreInvalidCustomers()
    {
        var customers = new List<Customer>
        {
            new Customer(-1, "name", "name", 18),
            new Customer(1, "", "", 19),
            new Customer(2, "name3", "name3", 6),
        };

        var existingIds = new int[] { 2, 3 };

        var errors = new List<string>();

        foreach (var customer in customers)
        {
            errors.AddRange(customer.Validate(existingIds));
        }

        _customerService.Setup(x => x.InsertCustomers(customers))
            .Returns(errors);

        _customerController = new CustomersController(_customerService.Object);
        var result = _customerController.Create(customers);

        result.Should().BeOfType<UnprocessableEntityObjectResult>();

        var objectResult = result as UnprocessableEntityObjectResult;
        objectResult.Value.Should().BeOfType<List<string>>();

        var customerList = objectResult.Value as List<string>;
        customerList.Should().NotBeNull().And.BeEquivalentTo(errors);
    }

    [Fact]
    public void Create_ShouldReturnEmptyOkIfThereAreNoErrors()
    {
        var customers = new List<Customer>
        {
            new Customer(1, "name", "name", 18),
            new Customer(2, "name2", "name2", 19),
            new Customer(3, "name3", "name3", 20),
        };

        var existingIds = new int[] { 4, 5, 6 };

        var errors = new List<string>();

        foreach (var customer in customers)
        {
            errors.AddRange(customer.Validate(existingIds));
        }

        _customerService.Setup(x => x.InsertCustomers(customers))
            .Returns(errors);

        _customerController = new CustomersController(_customerService.Object);
        var result = _customerController.Create(customers);

        result.Should().BeOfType<OkResult>();
    }
}

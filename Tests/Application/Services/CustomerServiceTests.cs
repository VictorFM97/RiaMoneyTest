using Application.Extensions;
using Application.Services;
using Domain.Customers;
using Persistence.Interfaces;

namespace Tests.Application.Services;
public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _customerRepository;
    private CustomerService _service;
    
    public CustomerServiceTests()
    {
        _customerRepository = new Mock<ICustomerRepository>();
    }

    [Fact]
    public void ConstructorAndGetAllCustomers_ShouldRetrieveSavedCustomersAndSortThem()
    {
        var customers = new List<Customer>
        {
            new Customer(1, "Abc", "zyx", 18),
            new Customer(2, "def", "abc", 18),
            new Customer(3, "aaa", "bbb", 18),
            new Customer(4, "aaa", "baa", 18),
        };

        var expectedCustomers = new Customer[4];
        customers.CopyTo(expectedCustomers);

        _customerRepository.Setup(x => x.GetAll())
            .Returns(customers);

        expectedCustomers.ToList().SortCustomers(0, customers.Count - 1);

        _service = new CustomerService(_customerRepository.Object);
        
        _customerRepository.Verify(x => x.GetAll(), Times.Once());

        var resultCustomers = _service.GetAllCustomers();
        resultCustomers.Should().BeEquivalentTo(expectedCustomers);
    }

    [Fact]
    public void InsertCustomers_ShouldNotInsertAnyCustomersIfOneIsInvalid()
    {
        var existingCustomers = new List<Customer>
        {
            new Customer(10, "Abc", "zyx", 18)
        };

        _customerRepository.Setup(x => x.GetAll())
            .Returns(existingCustomers);

        var customers = new List<Customer>
        {
            new Customer(1, "", "", 18),
            new Customer(2, "def", "abc", 18),
            new Customer(3, "aaa", "bbb", 18),
            new Customer(4, "aaa", "baa", 18),
        };

        var expectedErrors = customers.First().Validate(existingCustomers.Select(x => x.Id).ToArray());

        _service = new CustomerService(_customerRepository.Object);
        var errors = _service.InsertCustomers(customers);

        _customerRepository.Verify(x => x.Add(It.IsAny<Customer>()), Times.Never);
        errors.Should().NotBeNullOrEmpty()
            .And.BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public void InsertCustomers_ShouldInsertIfAllCustomersAreValid()
    {
        var existingCustomers = new List<Customer>
        {
            new Customer(10, "Abc", "zyx", 18)
        };

        _customerRepository.Setup(x => x.GetAll())
            .Returns(existingCustomers);

        var customers = new List<Customer>
        {
            new Customer(1, "aaa", "zyx", 18),
            new Customer(2, "def", "abc", 18),
            new Customer(3, "aaa", "bbb", 18),
            new Customer(4, "aaa", "baa", 18),
        };

        _service = new CustomerService(_customerRepository.Object);
        var errors = _service.InsertCustomers(customers);
        errors.Should().NotBeNull().And.BeEmpty();

        _customerRepository.Verify(x => x.Add(It.IsAny<Customer>()), Times.Exactly(4));
    }
}

using Application.Interfaces;
using Domain.Customers;
using Persistence.Interfaces;

namespace Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly List<Customer> _customers;

    public CustomerService(ICustomerRepository context)
    {
        _customerRepository = context;
        _customers = new List<Customer>();//_customerRepository.GetAllAsync().Result;
    }

    public List<Customer> GetAllCustomers()
    {
        return _customers;
    }

    public Task InsertCustomers(List<Customer> customers)
    {
        var errors = new List<string>();

        foreach(var customer in customers)
        {
            var customerErrors = customer.Validate(_customers.Select(x => x.Id).ToArray());

            if(customerErrors.Any())
            {
                errors.AddRange(customerErrors);
                continue;
            }

            //_customerRepository.Add(customer);
            _customers.InsertAtCorrectPosition(customer);
        }

        return Task.CompletedTask;
    }
}

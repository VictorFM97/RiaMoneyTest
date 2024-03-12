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
        _customers = _customerRepository.GetAllAsync().Result;
        _customers.SortCustomers(0, _customers.Count - 1);
    }

    public List<Customer> GetAllCustomers()
    {
        return _customers;
    }

    public List<string> InsertCustomers(List<Customer> customers)
    {
        var errors = new List<string>();

        var requestHasDuplicateIds = customers.DistinctBy(x => x.Id).Count() != customers.Count;

        if (requestHasDuplicateIds)
        {
            errors.Add("Request contain duplicate ids");
        }

        foreach(var customer in customers)
        {
            var customerErrors = customer.Validate(_customers.Select(x => x.Id).ToArray());

            if(customerErrors.Any())
            {
                errors.AddRange(customerErrors);
                continue;
            }

            _customerRepository.Add(customer);
            _customers.InsertionSort(customer);
        }

        return errors;
    }
}

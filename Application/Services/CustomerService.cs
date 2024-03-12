using Application.Extensions;
using Application.Interfaces;
using Application.ValueHolder;
using Domain.Customers;
using Persistence.Interfaces;

namespace Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository context)
    {
        _customerRepository = context;
        CustomerHolder.Customers = _customerRepository.GetAllAsync().Result;
        CustomerHolder.Customers.SortCustomers(0, CustomerHolder.Customers.Count - 1);
    }

    public List<Customer> GetAllCustomers()
    {
        return CustomerHolder.Customers;
    }

    public List<string> InsertCustomers(List<Customer> customers)
    {
        var errors = new List<string>();

        var requestHasDuplicateIds = customers.DistinctBy(x => x.Id).Count() != customers.Count;

        if (requestHasDuplicateIds)
        {
            errors.Add("Request contain duplicate ids");
        }

        var existingIds = CustomerHolder.Customers.Select(x => x.Id).ToArray();
        errors.AddRange(customers.SelectMany(x => x.Validate(existingIds)));

        if (!errors.Any())
        {
            foreach (var customer in customers)
            {
                _customerRepository.Add(customer);
                CustomerHolder.Customers.InsertionSort(customer);
            }
        }

        return errors;
    }
}

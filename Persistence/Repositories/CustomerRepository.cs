using Domain.Customers;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerContext _context;

    public CustomerRepository(CustomerContext customerContext)
    {
        _context = customerContext;
    }

    public void Add(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
    }

    public List<Customer> GetAll()
    {
        return _context.Customers.AsQueryable().ToList();
    }
}

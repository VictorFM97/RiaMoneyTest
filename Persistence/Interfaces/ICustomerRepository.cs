using Domain.Customers;

namespace Persistence.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();

    Task Add(Customer customer);
}

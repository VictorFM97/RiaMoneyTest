using Domain.Customers;

namespace Persistence.Interfaces;

public interface ICustomerRepository
{
    List<Customer> GetAll();

    void Add(Customer customer);
}

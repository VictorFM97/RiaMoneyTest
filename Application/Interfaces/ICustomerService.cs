using Domain.Customers;

namespace Application.Interfaces;

public interface ICustomerService
{
    List<Customer> GetAllCustomers();
    Task InsertCustomers(List<Customer> customers);
}

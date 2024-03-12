using Domain.Customers;

namespace Application.Interfaces;

public interface ICustomerService
{
    List<Customer> GetAllCustomers();
    List<string> InsertCustomers(List<Customer> customers);
}

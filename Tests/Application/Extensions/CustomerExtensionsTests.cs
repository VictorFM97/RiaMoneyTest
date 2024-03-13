using Application.Extensions;
using Domain.Customers;

namespace Tests.Application.Extensions;

public class CustomerExtensionsTests
{
    [Fact]
    public void InsertionSort_ShouldInsertNewCustomerInCorrectPosition()
    {
        var existingCustomers = new List<Customer>
        {
            new Customer(1, "aaa", "zyx", 18),
            new Customer(2, "def", "abc", 18),
            new Customer(3, "aaa", "bbb", 18),
            new Customer(4, "aaa", "baa", 18),
            new Customer(5, "aac", "bbb", 20)
        };

        existingCustomers.SortCustomers(0, existingCustomers.Count - 1);

        var customerToBeAdded = new Customer(6, "aaa", "acb", 20);

        existingCustomers.InsertionSort(customerToBeAdded);

        var expectedCustomers = new List<Customer>
        {
            new Customer(2, "def", "abc", 18),
            customerToBeAdded,
            new Customer(4, "aaa", "baa", 18),
            new Customer(3, "aaa", "bbb", 18),
            new Customer(5, "aac", "bbb", 20),
            new Customer(1, "aaa", "zyx", 18),
        };

        existingCustomers.Should().BeEquivalentTo(expectedCustomers, options => options.WithStrictOrdering());
    }

    [Fact]
    public void SortCustomers_ShouldSortCustomersCorrectly()
    {
        var existingCustomers = new List<Customer>
        {
            new Customer(1, "aaa", "zyx", 18),
            new Customer(2, "def", "abc", 18),
            new Customer(3, "aaa", "bbb", 18),
            new Customer(4, "aaa", "baa", 18),
            new Customer(5, "aac", "bbb", 20)
        };

        existingCustomers.SortCustomers(0, existingCustomers.Count - 1);

        var expectedCustomers = new List<Customer>
        {
            new Customer(2, "def", "abc", 18),
            new Customer(4, "aaa", "baa", 18),
            new Customer(3, "aaa", "bbb", 18),
            new Customer(5, "aac", "bbb", 20),
            new Customer(1, "aaa", "zyx", 18),
        };

        existingCustomers.Should().BeEquivalentTo(expectedCustomers, options => options.WithStrictOrdering());
    }
}

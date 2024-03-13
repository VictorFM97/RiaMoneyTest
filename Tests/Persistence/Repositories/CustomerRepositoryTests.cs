using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Persistence;
using Persistence.Repositories;

namespace Tests.Persistence.Repositories;
public class CustomerRepositoryTests
{
    private readonly Mock<CustomerContext> _context;
    private readonly Mock<DbSet<Customer>> _customerDbSet;
    private readonly List<Customer> _existingCustomers;
    private CustomerRepository _customerRepository;

    public CustomerRepositoryTests()
    {
        _customerDbSet = new Mock<DbSet<Customer>>();

        _context = new Mock<CustomerContext>();
        _context.Setup(x => x.Set<Customer>()).Returns(_customerDbSet.Object);
        _context.Setup(x => x.Customers).Returns(_customerDbSet.Object);

        _existingCustomers = new List<Customer>
        {
            new Customer(1, "aaa", "zyx", 18),
            new Customer(2, "def", "abc", 18),
            new Customer(3, "aaa", "bbb", 18),
            new Customer(4, "aaa", "baa", 18),
            new Customer(5, "aac", "bbb", 20)
        };

        _customerDbSet.Setup(x => x.AsQueryable())
            .Returns(_existingCustomers.AsQueryable().BuildMockDbSet().Object);
    }

    [Fact]
    public void Add_ShouldAddAndCallSaveChanges()
    {
        var customer = new Customer(6, "firstname", "lastname", 18);
        _customerRepository = new CustomerRepository(_context.Object);
        _customerRepository.Add(customer);

        _customerDbSet.Verify(x => x.Add(customer), Times.Once());
        _context.Verify(x => x.SaveChanges(), Times.Once());
    }

    [Fact]
    public void GetAllAsync_ShouldReturnExistingListOfCustomers()
    {
        _customerRepository = new CustomerRepository(_context.Object);

        var result = _customerRepository.GetAll();

        result.Should().BeEquivalentTo(_existingCustomers);
    }
}

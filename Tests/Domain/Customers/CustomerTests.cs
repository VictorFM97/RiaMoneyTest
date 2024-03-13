using Domain.Customers;

namespace Tests.Domain.Customers;

public class CustomerTests
{
    [Fact]
    public void Constructor_ShouldCreateCorrectCustomer()
    {
        var customer = new Customer(1, "firstname", "lastname", 14);
        customer.Id.Should().Be(1);
        customer.FirstName.Should().Be("firstname");
        customer.LastName.Should().Be("lastname");
        customer.Age.Should().Be(14);
    }

    [Fact]
    public void Validate_ShouldReturnEmptyIfCustomerIsValid()
    {
        var customer = new Customer(1, "firstname", "lastname", 18);
        var existingIds = new int[] { 2, 3 };
        var errors = customer.Validate(existingIds);

        errors.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void Validate_ShouldReturnAllErrorsIfCustomerIsInvalid()
    {
        var customer = new Customer(-1, "", "", 4);
        var existingIds = new int[] { -1, 1, 2 };
        var errors = customer.Validate(existingIds);

        var expectedErrors = new string[]
        {
            $"Id cannot be 0 or negative, Id: {customer.Id}",
            $"Id already exists, Id: {customer.Id}",
            $"First Name cannot be null or empty, Id: {customer.Id}",
            $"Last Name cannot be null or empty, Id: {customer.Id}",
            $"Must be older than 18 to register, Id: {customer.Id}"
        };

        errors.Should().NotBeNullOrEmpty();
        foreach(var expectedError in expectedErrors)
        {
            errors.Should().Contain(expectedError);
        }
    }

    [Theory]
    [InlineData("AAA", "ABB", "AAA", "BBB")]
    [InlineData("AAA", "ABB", "ABC", "ABB")]
    public void CompareNames_ShouldReturnMinus1IfNameIsLowerThan(
        string firstName1, 
        string lastName1, 
        string firstName2, 
        string lastName2)
    {
        var customer1 = new Customer(1, firstName1, lastName1, 18);
        var customer2 = new Customer(2, firstName2, lastName2, 18);

        customer1.CompareNames(customer2).Should().Be(-1);
    }

    [Theory]
    [InlineData("AAA", "BBB", "AAA", "ABB")]
    [InlineData("ABC", "BBB", "AAB", "BBB")]
    public void CompareNames_ShouldReturn1IfNameIsHigherThan(
        string firstName1,
        string lastName1,
        string firstName2,
        string lastName2)
    {
        var customer1 = new Customer(1, firstName1, lastName1, 18);
        var customer2 = new Customer(2, firstName2, lastName2, 18);

        customer1.CompareNames(customer2).Should().Be(1);
    }

    [Fact]
    public void CompareNames_ShouldReturn0IfNamesAreIdentical()
    {
        var customer1 = new Customer(1, "A", "A", 18);
        var customer2 = new Customer(2, "A", "A", 18);

        customer1.CompareNames(customer2).Should().Be(0);
    }
}

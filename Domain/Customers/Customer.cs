namespace Domain.Customers;

public class Customer
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public int Age { get; private set; }

    public Customer(int id, string firstName, string lastName, int age)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Age = age;
    }

    public List<string> Validate(int[] existingIds)
    {
        var errors = new List<string>();

        if(Id < 0)
        {
            errors.Add($"Id cannot be 0 or negative, Id: {Id}");
        }
        else if (existingIds.Contains(Id))
        {
            errors.Add($"Id already exists, Id: {Id}");
        }

        if (string.IsNullOrEmpty(FirstName))
        {
            errors.Add($"First Name cannot be null or empty, Id: {Id}");
        }

        if (string.IsNullOrEmpty(LastName))
        {
            errors.Add($"Last Name cannot be null or empty, Id: {Id}");
        }

        if (Age < 18)
        {
            errors.Add($"Must be older than 18 to register, Id: {Id}");
        }

        return errors;
    }

    public int CompareNames(Customer otherCustomer)
    {
        var lastNameComparison = LastName.CompareTo(otherCustomer.LastName);

        if(lastNameComparison != 0)
        {
            return lastNameComparison;
        }

        return FirstName.CompareTo(otherCustomer.FirstName); 
    }
}

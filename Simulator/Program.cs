using Domain.Customers;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

internal class Program
{
    private static List<string> FirstNames = new List<string>
    {
        "Leia", "Sadie", "Jose", "Sara", "Frank", 
        "Dewey", "Tomas", "Joel", "Lukas", "Carlos"
    };

    private static List<string> LastNames = new List<string>
    {
        "Liberty", "Ray", "Harrison", "Ronan", "Drew",
        "Powell", "Larsen", "Chan", "Anderson", "Lane"
    };

    private static Uri ApiUri = new Uri("http://host.docker.internal:45600");

    private static async Task Main(string[] args)
    {
        int amount = new Random().Next(10, 40);

        var addTask = AddCustomers(amount);

        // Adding small delay so it prints some customers
        await Task.Delay(300);
        var getTask = GetCustomers(amount);

        await Task.WhenAll(addTask, getTask);

        Console.WriteLine("Finished processing.");
    }

    private static async Task AddCustomers(int amount)
    {
        var random = new Random();
        int id = 1;

        for(int i = 0; i < amount; i++)
        {
            var customers = new List<Customer>();

            var amountOfCustomers = random.Next(2, 6);

            for (int j = 0; j < amountOfCustomers; j++)
            {
                var customer = new Customer(id, FirstNames[random.Next(0, 9)], LastNames[random.Next(0, 9)], random.Next(10, 90));
                customers.Add(customer);
                id++;
            }

            await PostCustomers(customers);
        }

        await Task.CompletedTask;
    }

    private static async Task PostCustomers(List<Customer> customers)
    {
        var client = new HttpClient
        {
            BaseAddress = ApiUri
        };

        using StringContent requestBody = new(
            JsonSerializer.Serialize(customers),
            Encoding.UTF8,
            "application/json");

        try
        {
            var response = await client.PostAsync("/customers", requestBody);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var errors = await response.Content.ReadFromJsonAsync<List<string>>();

                errors!.ForEach(Console.WriteLine);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message} inner: {ex.InnerException.Message}");
        }
    }

    private static async Task GetCustomers(int amount)
    {
        var client = new HttpClient
        {
            BaseAddress = ApiUri
        };

        for (int i = 0; i < amount; i++) 
        {
            var response = await client.GetAsync("/customers");
            var customers = await response.Content.ReadFromJsonAsync<List<Customer>>();

            foreach(var customer in customers!)
            {
                Console.WriteLine(
                    $"Id: {customer.Id}, First Name: {customer.FirstName}, Last Name: {customer.LastName}, Age: {customer.Age}");
            }
        }

        Console.WriteLine("------------------------------------------");
    }
}

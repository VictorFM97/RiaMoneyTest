using Domain.Customers;
using System.Diagnostics.CodeAnalysis;

namespace Application.ValueHolder;

[ExcludeFromCodeCoverage]
public static class CustomerHolder
{
    public static List<Customer> Customers;
}

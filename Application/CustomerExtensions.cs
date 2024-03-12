using Domain.Customers;

namespace Application;

public static class CustomerExtensions
{
    public static void InsertionSort(this List<Customer> list, Customer customerToAdd)
    {
        int index = 0;

        int high = list.Count - 1, low = 0;

        while (low <= high)
        {
            int mid = (high + low) / 2;
            var comparison = customerToAdd.CompareNames(list[mid]);

            if (comparison < 0)
            {
                high = mid - 1;
            }
            else if (comparison > 0)
            {
                low = mid + 1;
            }
            else
            {
                index = mid + 1;
                break;
            }
        }

        if (index == 0)
            index = low;

        list.Insert(index, customerToAdd);
    }
}

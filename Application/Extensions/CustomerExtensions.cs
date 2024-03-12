using Domain.Customers;

namespace Application.Extensions;

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

    public static void SortCustomers(this List<Customer> array, int leftIndex, int rightIndex)
    {
        if (array.Count == 0)
        {
            return;
        }

        var i = leftIndex;
        var j = rightIndex;
        var pivot = array[leftIndex];
        while (i <= j)
        {
            while (array[i].CompareNames(pivot) < 0)
            {
                i++;
            }

            while (array[j].CompareNames(pivot) > 0)
            {
                j--;
            }

            if (i <= j)
            {
                Customer temp = array[i];
                array[i] = array[j];
                array[j] = temp;
                i++;
                j--;
            }
        }

        if (leftIndex < j)
            array.SortCustomers(leftIndex, j);
        if (i < rightIndex)
            array.SortCustomers(i, rightIndex);
    }
}

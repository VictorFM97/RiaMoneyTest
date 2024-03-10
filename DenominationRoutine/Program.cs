internal class Program
{
    private static void Main(string[] args)
    {
        var denominations = new int[] { 100, 50, 10 }.OrderByDescending(x => x).ToArray();

        var amountsToPrint = new int[]
        {
            30,
            50,
            60,
            80,
            140,
            230,
            370,
            610,
            980
        };

        foreach (var amount in amountsToPrint)
        {
            if (amount % 10 != 0)
            {
                Console.WriteLine("Impossible to create combination.");
            }

            Console.WriteLine($"Combinations for: ${amount}:");

            var result = new List<DenominationResult>();

            foreach (var denomination in denominations)
            {
                GetCombinations(amount, denomination, result);
            }

            foreach (var combination in result)
            {
                var combinationString = "";

                foreach (var det in combination.Combinations)
                {
                    combinationString = string.Concat(combinationString, $"{det.Key} X {det.Value} EUR + ");
                }

                Console.WriteLine(combinationString.TrimEnd('+', ' '));
                Console.WriteLine("------------");
            }

            Console.WriteLine("-------------------------------------------");
        }
    }

    private static void GetCombinations(int amount, int denomination, List<DenominationResult> results)
    {
        if (amount < denomination)
        {
            return;
        }

        foreach (var combination in results.Where(x => x.MissingAmount > 0))
        {
            var notesForCombination = combination.MissingAmount / denomination;
            combination.Combinations.Add(denomination, notesForCombination);
        }

        var notes = amount / denomination;
        var missingAmount = amount - (denomination * notes);

        if (missingAmount == 0)
        {
            results.Add(new DenominationResult
            {
                MissingAmount = missingAmount,
                Combinations = new Dictionary<int, int> { { denomination, notes } }
            });
        }
        else
        {
            for (int i = notes; i > 0; i--)
            {
                results.Add(new DenominationResult
                {
                    MissingAmount = amount - (i * denomination),
                    Combinations = new Dictionary<int, int>
                    {
                        { denomination, i }
                    }
                });
            }
        }
    }
}

public class DenominationResult
{
    public int MissingAmount { get; set; }
    public Dictionary<int, int> Combinations { get; set; }
}
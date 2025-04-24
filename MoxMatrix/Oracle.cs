using System.Globalization;
using static System.Windows.Forms.LinkLabel;

namespace MoxMatrix
{
  public class Oracle
  {
    private const decimal DeliveryCost = 100m;

    public void OptimisePurchases(string[] inputCsvLines, string outputTextPath)
    {
      if (inputCsvLines.Length < 2)
        throw new InvalidOperationException("CSV does not contain enough data.");

      var headers = inputCsvLines[0].Split(';');
      var storeNames = headers.Skip(1).ToList();

      var cardRows = inputCsvLines
          .Skip(1)
          .Where(line => !line.StartsWith("Total Price"))
          .Select(line => line.Split(';'))
          .ToList();

      var storeCards = new Dictionary<string, List<(string cardName, decimal price)>>();
      var usedStores = new HashSet<string>();
      var totalCost = 0m;

      foreach (var row in cardRows)
      {
        var cardName = row[0];
        var minEffectiveCost = decimal.MaxValue;
        var bestStoreIndex = -1;

        for (var i = 1; i < row.Length; i++)
        {
          var rawValue = row[i].Replace("✨", "").Trim();
          if (!decimal.TryParse(rawValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var price))
            continue;

          var effectiveCost = price + (usedStores.Contains(storeNames[i - 1]) ? 0 : DeliveryCost);

          if (effectiveCost < minEffectiveCost)
          {
            minEffectiveCost = effectiveCost;
            bestStoreIndex = i - 1;
          }
        }

        if (bestStoreIndex >= 0)
        {
          var store = storeNames[bestStoreIndex];
          var price = decimal.Parse(row[bestStoreIndex + 1].Replace("✨", "").Trim(), CultureInfo.InvariantCulture);

          if (!storeCards.ContainsKey(store))
            storeCards[store] = new List<(string, decimal)>();

          storeCards[store].Add((cardName, price));

          if (!usedStores.Contains(store))
          {
            totalCost += DeliveryCost;
            usedStores.Add(store);
          }
          totalCost += price;
        }
      }

      using var writer = new StreamWriter(outputTextPath);
      foreach (var store in storeCards.Keys.OrderBy(k => k))
      {
        writer.WriteLine($"Store: {store}");
        decimal storeTotal = 0;
        foreach (var (card, price) in storeCards[store])
        {
          writer.WriteLine($"  - {card}: R{price}");
          storeTotal += price;
        }
        writer.WriteLine($"  Delivery: R{DeliveryCost}");
        writer.WriteLine($"  Subtotal: R{storeTotal + DeliveryCost}\n");
      }
      writer.WriteLine($"Total Cost: R{totalCost}");
    }

    public void OptimisePurchases(string inputCsvPath, string outputTextPath)
      => OptimisePurchases(File.ReadAllLines(inputCsvPath), outputTextPath);
  }

}

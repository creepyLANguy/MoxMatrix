using System.Globalization;

namespace MoxMatrix
{
  public static class Oracle_v2
  {
    private const decimal DeliveryCost = 100m;

    private static Tuple<Dictionary<string, List<(string cardName, decimal price)>>, decimal> GetOptimisedPurchases(string[] inputCsvLines)
    {
      if (inputCsvLines.Length < 2)
      {
        throw new InvalidOperationException("CSV does not contain enough data.");
      }

      var headers = inputCsvLines[0].Split(new List<char> { ';' }.ToArray());
      var storeNames = headers.Skip(1).ToList();

      var cardRows = inputCsvLines
        .Skip(1)
        .Where(line => !line.StartsWith("Total Price"))
        .Select(line => line.Split(';'))
        .ToList();

      var storeCards = new Dictionary<string, List<(string cardName, decimal price)>>();
      var usedStores = new HashSet<string>();
      var totalCost = 0m;

      var cardAssignments = new Dictionary<string, (string store, decimal price)>();

      foreach (var row in cardRows)
      {
        var cardName = row[0];
        var minEffectiveCost = decimal.MaxValue;
        var bestStoreIndex = -1;

        for (var i = 1; i < row.Length; i++)
        {
          var rawValue = row[i].Replace("✨", "").Trim();
          if (!decimal.TryParse(rawValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var currentPrice))
          {
            continue;
          }

          var effectiveCost = currentPrice + (usedStores.Contains(storeNames[i - 1]) ? 0 : DeliveryCost);

          if (effectiveCost < minEffectiveCost)
          {
            minEffectiveCost = effectiveCost;
            bestStoreIndex = i - 1;
          }
        }

        if (bestStoreIndex < 0)
        {
          continue;
        }

        var store = storeNames[bestStoreIndex];
        var price = decimal.Parse(row[bestStoreIndex + 1].Replace("✨", "").Trim(), CultureInfo.InvariantCulture);

        cardAssignments[cardName] = (store, price);

        if (!storeCards.ContainsKey(store))
        {
          storeCards[store] = new List<(string, decimal)>();
        }
        storeCards[store].Add((cardName, price));

        if (!usedStores.Contains(store))
        {
          totalCost += DeliveryCost;
          usedStores.Add(store);
        }
        totalCost += price;
      }

      ApplyPostProcessing(cardRows, storeNames, ref storeCards, ref usedStores, ref totalCost, ref cardAssignments);

      return Tuple.Create(storeCards, totalCost);
    }

    private static void ApplyPostProcessing(List<string[]> cardRows, List<string> storeNames,
        ref Dictionary<string, List<(string cardName, decimal price)>> storeCards,
        ref HashSet<string> usedStores,
        ref decimal totalCost,
        ref Dictionary<string, (string store, decimal price)> cardAssignments)
    {
      foreach (var row in cardRows)
      {
        var cardName = row[0];
        if (!cardAssignments.ContainsKey(cardName)) continue;

        var (currentStore, currentPrice) = cardAssignments[cardName];
        decimal currentTotalImpact = currentPrice;
        if (storeCards[currentStore].Count == 1) currentTotalImpact += DeliveryCost;

        foreach (var i in Enumerable.Range(1, row.Length - 1))
        {
          var rawValue = row[i].Replace("✨", "").Trim();
          if (!decimal.TryParse(rawValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var newPrice))
          {
            continue;
          }

          var altStore = storeNames[i - 1];
          if (altStore == currentStore) continue;

          var alreadyUsed = usedStores.Contains(altStore);
          var deliveryPenalty = alreadyUsed ? 0 : DeliveryCost;

          decimal newTotalImpact = newPrice + deliveryPenalty;

          if (newTotalImpact + 0.01m < currentTotalImpact) // allow small tolerance
          {
            // Update assignment
            storeCards[currentStore].RemoveAll(x => x.cardName == cardName);
            if (storeCards[currentStore].Count == 0)
            {
              storeCards.Remove(currentStore);
              usedStores.Remove(currentStore);
              totalCost -= DeliveryCost;
            }
            totalCost -= currentPrice;

            if (!storeCards.ContainsKey(altStore))
              storeCards[altStore] = new List<(string, decimal)>();

            storeCards[altStore].Add((cardName, newPrice));
            if (!alreadyUsed)
            {
              usedStores.Add(altStore);
              totalCost += DeliveryCost;
            }
            totalCost += newPrice;

            cardAssignments[cardName] = (altStore, newPrice);
            break;
          }
        }
      }
    }

    public static void ExportBuyList(string inputCsvPath, string outputTextPath)
      => ExportBuyList(File.ReadAllLines(inputCsvPath), outputTextPath);

    public static void ExportBuyList(string[] inputCsvLines, string outputTextPath)
    {
      var x = GetOptimisedPurchases(inputCsvLines);
      PerformFinalExport(x.Item1, x.Item2, outputTextPath);
    }

    private static void PerformFinalExport(Dictionary<string, List<(string cardName, decimal price)>> storeCards, decimal totalCost, string outputTextPath)
    {
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
  }
}

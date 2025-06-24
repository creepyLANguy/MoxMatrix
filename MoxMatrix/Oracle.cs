//Version: 4.0
//Date: 14/05/2025

using System.Globalization;

namespace MoxMatrix
{
  public static class Oracle
  {
    private const decimal DeliveryCost = 100m;

    private static Tuple<decimal, Dictionary<string, List<(string cardName, decimal price)>>> GetOptimisedPurchases(
      string[] inputCsvLines,
      int maxPreferredStores)
    {
      if (inputCsvLines.Length < 2)
      {
        throw new InvalidOperationException("CSV does not contain enough data.");
      }

      var headers = inputCsvLines[0].Split(new List<char> {';'}.ToArray());
      var storeNames = headers.Skip(1).ToList();

      var cardRows = inputCsvLines
        .Skip(1)
        .Where(line => !line.StartsWith("Total Price"))
        .Select(line => line.Split(';'))
        .ToList();

      var storeAvailability = new Dictionary<string, int>();
      foreach (var row in cardRows)
      {
        for (var i = 1; i < row.Length; i++)
        {
          if (string.IsNullOrWhiteSpace(row[i].Replace("✨", "").Trim()))
          {
            continue;
          }

          var store = storeNames[i - 1];
          storeAvailability.TryAdd(store, 0);
          storeAvailability[store]++;
        }
      }

      var preferredStores = storeAvailability
        .OrderByDescending(kvp => kvp.Value)
        .Take(Math.Min(maxPreferredStores, storeAvailability.Count))
        .Select(kvp => kvp.Key)
        .ToHashSet();

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
          var store = storeNames[i - 1];
          if (!preferredStores.Contains(store))
          {
            continue;
          }

          var rawValue = row[i].Replace("✨", "").Trim();
          if (!decimal.TryParse(rawValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var currentPrice))
          {
            continue;
          }

          var effectiveCost = currentPrice + (usedStores.Contains(store) ? 0 : DeliveryCost);

          if (effectiveCost >= minEffectiveCost)
          {
            continue;
          }

          minEffectiveCost = effectiveCost;
          bestStoreIndex = i - 1;
        }

        if (bestStoreIndex < 0)
        {
          continue;
        }

        var storeName = storeNames[bestStoreIndex];
        var price = decimal.Parse(row[bestStoreIndex + 1].Replace("✨", "").Trim());

        cardAssignments[cardName] = (storeName, price);

        if (!storeCards.ContainsKey(storeName))
        {
          storeCards[storeName] = new List<(string, decimal)>();
        }

        storeCards[storeName].Add((cardName, price));

        if (!usedStores.Contains(storeName))
        {
          totalCost += DeliveryCost;
          usedStores.Add(storeName);
        }

        totalCost += price;
      }

      bool changed;
      do
      {
        changed =
          ApplyPostProcessing(
            cardRows,
            storeNames,
            ref storeCards,
            ref usedStores,
            ref totalCost,
            ref cardAssignments);
      } while (changed);

      return Tuple.Create(totalCost, storeCards);
    }

    private static bool ApplyPostProcessing(List<string[]> cardRows, List<string> storeNames,
      ref Dictionary<string, List<(string cardName, decimal price)>> storeCards,
      ref HashSet<string> usedStores,
      ref decimal totalCost,
      ref Dictionary<string, (string store, decimal price)> cardAssignments)
    {
      var changed = false;

      foreach (var row in cardRows)
      {
        var cardName = row[0];
        if (!cardAssignments.ContainsKey(cardName)) continue;

        var (currentStore, currentPrice) = cardAssignments[cardName];
        var currentTotalImpact = currentPrice;
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


          if (newPrice + deliveryPenalty + 0.01m >= currentTotalImpact)
          {
            continue;
          }

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
          changed = true;
          break;
        }
      }

      return changed;
    }

    public static void ExportBuyList(string[] inputCsvLines, string outputTextPath, int maxPreferredStores)
    {
      var optimisedPurchasesResult = GetOptimisedPurchases(inputCsvLines, maxPreferredStores);
      Exporter.Run("Oracle_v4",
        optimisedPurchasesResult.Item1,
        optimisedPurchasesResult.Item2,
        DeliveryCost,
        outputTextPath,
        -1, //TODO - take a list of string and display them
        -1, //TODO - take a list of string and display them
        -1,
        -1,
        -1,
        -1,
        -1);
    }
  }
}
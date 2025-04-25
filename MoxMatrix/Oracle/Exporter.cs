namespace MoxMatrix;

public static class Exporter
{
  public static void Run(Dictionary<string, List<(string cardName, decimal price)>> storeCards,
    decimal totalCost, string algorithmVersion, decimal deliveryCost, string outputTextPath)
  {
    using var writer = new StreamWriter(outputTextPath);

    var cardsCount = storeCards.Sum(storeCard => storeCard.Value.Count);

    writer.WriteLine($"Algorithm Version: {algorithmVersion}");
    writer.WriteLine($"Total Cards: {cardsCount}");
    writer.WriteLine($"Total Cost: R{totalCost}");
    writer.WriteLine();

    //AL.
    //TODO - take individual store delivery costs into account
    //TODO - show list of missing/ommitted cards

    foreach (var store in storeCards.Keys.OrderBy(k => k))
    {
      writer.WriteLine($"Store: {store}");
      decimal storeTotal = 0;
      foreach (var (card, price) in storeCards[store])
      {
        writer.WriteLine($"  - {card}: R{price}");
        storeTotal += price;
      }

      writer.WriteLine($"  Delivery: R{deliveryCost}");
      writer.WriteLine($"  Subtotal: R{storeTotal + deliveryCost}\n");
    }
  }
}
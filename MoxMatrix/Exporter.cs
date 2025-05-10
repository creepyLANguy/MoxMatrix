namespace MoxMatrix;

public static class Exporter
{
  public static void Run(
    string algorithmVersion,
    decimal totalCost,
    Dictionary<string, List<(string cardName, decimal price)>> storesAndTheirCards,
    decimal deliveryCost,
    string outputTextPath,
    int cardsOmitted,
    int cardsSoldOut,
    int cardsInput,
    int cardsKnown,
    int cardsUnknown,
    int storesConsidered,
    int storesWithStock)
  {
    using var writer = new StreamWriter(outputTextPath);

    var cardsChosen = storesAndTheirCards.Sum(satc => satc.Value.Count);
    var storesUtilised = storesAndTheirCards.Keys.Count;

    writer.WriteLine($"Algorithm Version:{algorithmVersion}");
    writer.WriteLine();
    writer.WriteLine($"Total Cost:\tR{totalCost}");
    writer.WriteLine();
    writer.WriteLine($"Cards Chosen:\t{cardsChosen}");
    writer.WriteLine($"Cards Omitted:\t{cardsOmitted}");
    writer.WriteLine($"Cards Sold Out:\t{cardsSoldOut}");
    writer.WriteLine();
    writer.WriteLine($"Cards Input:\t{cardsInput}");
    writer.WriteLine($"Cards Known:\t{cardsKnown}");
    writer.WriteLine($"Cards Unknown:\t{cardsUnknown}");
    writer.WriteLine();
    writer.WriteLine($"Stores Utilised:\t{storesUtilised}");
    writer.WriteLine($"Stores Considered:\t{storesConsidered}");
    writer.WriteLine($"Stores With Stock:\t{storesWithStock}");
    writer.WriteLine();

    foreach (var store in storesAndTheirCards.Keys.OrderBy(k => k))
    {
      writer.WriteLine($"Store: {store}");
      decimal storeTotal = 0;
      foreach (var (card, price) in storesAndTheirCards[store])
      {
        writer.WriteLine($"  ⭕ {card}: R{price}");
        storeTotal += price;
      }

      writer.WriteLine($"  Delivery:\tR{deliveryCost}");
      writer.WriteLine($"  Subtotal:\tR{storeTotal + deliveryCost}\n");
    }
  }
}
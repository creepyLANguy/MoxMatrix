using Newtonsoft.Json;

namespace MoxMatrix
{
  public partial class Form1 : Form
  {
    public class Card
    {
      public string Id { get; set; }
      public string Name { get; set; }
      public DateTime LastScraped { get; set; }
      public bool IsScraping { get; set; }
    }

    public class CardsResponse
    {
      public List<Card> Cards { get; set; }
    }

    private const string CardMatchEndpoint = "https://moxmonolith.com/card/search?name=";

    private const string PricesEndpoint = "https://moxmonolith.com/card/{id}/products";

    private const string RetailersFilter =
      "?retailers[]=2&retailers[]=3&retailers[]=4&retailers[]=6&retailers[]=11&retailers[]=13&retailers[]=15&retailers[]=16&retailers[]=18&retailers[]=19&retailers[]=20&retailers[]=21&retailers[]=26&retailers[]=34&retailers[]=41&retailers[]=44";

    public class Product
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public int? Price { get; set; }
      public string PriceRead { get; set; }
      public string Link { get; set; }
      public int Stock { get; set; }
      public bool IsFoil { get; set; }
      public DateTime LastScraped { get; set; }
      public string Image { get; set; }
      public int RetailerId { get; set; }
      public string RetailerName { get; set; }
    }

    public class PriceResponse
    {
      public List<Product> Products { get; set; }
      public Card Card { get; set; }
    }


    private readonly string[] BlackListTerms = { "art card" };

    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      //AL.
      //DEBUG
      inputBox.Text += "mox" + Environment.NewLine;
      inputBox.Text += "opportunis" + Environment.NewLine;
      inputBox.Text += "countersp" + Environment.NewLine;
      inputBox.Text += "dreamtide" + Environment.NewLine;
      inputBox.Text += "asdasfdsfserf" + Environment.NewLine;
      //btn_go_Click(btn_go, null);
      //
    }

    private async void btn_go_Click(object sender, EventArgs e)
    {
      Text += " - Processing...";
      btn_go.Text = @"Querying prices from each store...";
      Enabled = false;

      var cardMatches = await GetCardMatchesListAsync();

      var priceList = await GetPriceListAsync(cardMatches);

      //AL.
      //Do something with the price list.


      Text = Text.Replace(" - Processing...", "");
      btn_go.Text = @"Go!";
      Enabled = true;
    }

    public async Task<List<Card>> GetCardMatchesListAsync()
    {
      List<Card> cardMatches = new List<Card>();

      var cardsInput = inputBox.Text.Split(Environment.NewLine).Where(it => it.Length > 0);
      var tasks = cardsInput.Select(input => GetCardMatchAsync(input)).ToList();

      var results = await Task.WhenAll(tasks);

      foreach (var match in results)
      {
        if (match != null)
        {
          cardMatches.Add(match);
        }
      }

      return cardMatches;
    }

    private async Task<Card?> GetCardMatchAsync(string input)
    {
      using var httpClient = new HttpClient();
      var uri = CardMatchEndpoint + input;
      var response = await httpClient.GetAsync(uri);
      var results = await response.Content.ReadAsStringAsync();
      var cardsResponse = JsonConvert.DeserializeObject<CardsResponse>(results);

      if (cardsResponse == null)
      {
        return null;
      }

      var matchEvaluations = cardsResponse.Cards
        .Select(card => GetMatchEvaluation(input, card.Name))
        .ToList();

      try
      {
        return cardsResponse.Cards[matchEvaluations.IndexOf(matchEvaluations.Min())];
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return null;
      }
    }

    int GetMatchEvaluation(string source1, string source2)
    {
      var source1Length = source1.Length;
      var source2Length = source2.Length;

      if (source1Length == 0)
        return source2Length;

      if (source2Length == 0)
        return source1Length;

      var matrix = new int[source1Length + 1, source2Length + 1];

      for (var i = 0; i <= source1Length; matrix[i, 0] = i++) { }
      for (var j = 0; j <= source2Length; matrix[0, j] = j++) { }

      for (var i = 1; i <= source1Length; i++)
      {
        for (var j = 1; j <= source2Length; j++)
        {
          var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

          matrix[i, j] = Math.Min(
            Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
            matrix[i - 1, j - 1] + cost);
        }
      }

      return matrix[source1Length, source2Length];
    }


    public async Task<List<PriceResponse>> GetPriceListAsync(List<Card> cardMatches)
    {
      var tasks = cardMatches.Select(cardMatch => GetPriceAsync(cardMatch)).ToList();
      var results = await Task.WhenAll(tasks);
      return results.Where(result => result != null).ToList();
    }

    private async Task<PriceResponse?> GetPriceAsync(Card cardMatch)
    {
      using var httpClient = new HttpClient();
      var uri = PricesEndpoint.Replace("{id}", cardMatch.Id) + RetailersFilter;
      var response = await httpClient.GetAsync(uri);
      var results = await response.Content.ReadAsStringAsync();
      var pricesResponse = JsonConvert.DeserializeObject<PriceResponse>(results);
      return pricesResponse;
    }

    //AL.
    //Use this to hide things like art cards
    bool ShouldShowProduct(Product product)
    {
      foreach (var blackListTerm in BlackListTerms) 
      {
        if (product.Name.ToLower().Contains(blackListTerm))
        {
          return false;
        }
      }

      return true;
    }
  }
}
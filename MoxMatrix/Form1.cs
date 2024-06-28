using Newtonsoft.Json;
using System.Text;
using static MoxMatrix.Form1;

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
      public int Retailer_Id { get; set; }
      public string Retailer_Name { get; set; }
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
      inputBox.Text += @"mox" + Environment.NewLine;
      inputBox.Text += @"opportunis" + Environment.NewLine;
      inputBox.Text += @"countersp" + Environment.NewLine;
      inputBox.Text += @"dreamtide" + Environment.NewLine;
      inputBox.Text += @"asdasfdsfserf" + Environment.NewLine;
      inputBox.Text += @"godtoucher" + Environment.NewLine;
      inputBox.Text += @"esper sentinal" + Environment.NewLine;
    }

    private async void btn_go_Click(object sender, EventArgs e)
    {
      const string buttonDefault = @"Go!";
      const string processingText = @" - Processing...";

      Text += processingText;
      btn_go.Text = @"Querying prices from each store...";
      Enabled = false;

      var cardMatches = await GetCardMatchesListAsync();

      var priceList = await GetPriceListAsync(cardMatches);

      GenerateCsv(priceList);

      Text = Text.Replace(processingText, string.Empty);
      btn_go.Text = buttonDefault;
      Enabled = true;
    }

    public async Task<List<Card>> GetCardMatchesListAsync()
    {
      var cardsInput = inputBox.Text.Split(Environment.NewLine).Where(it => it.Length > 0);
      var tasks = cardsInput.Select(GetCardMatchAsync).ToList();

      var results = await Task.WhenAll(tasks);

      return results.OfType<Card>().ToList();
    }

    private static async Task<Card?> GetCardMatchAsync(string input)
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

    private static int GetMatchEvaluation(string source1, string source2)
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
      var tasks = cardMatches.Select(GetPriceAsync).ToList();
      var results = await Task.WhenAll(tasks);
      return results.Where(result => result != null).ToList();
    }

    private static async Task<PriceResponse?> GetPriceAsync(Card cardMatch)
    {
      using var httpClient = new HttpClient();
      var uri = PricesEndpoint.Replace("{id}", cardMatch.Id) + RetailersFilter;
      var response = await httpClient.GetAsync(uri);
      var results = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<PriceResponse>(results);
    }

    private void GenerateCsv(List<PriceResponse> priceResponses)
    {
      var cheapestProducts = new Dictionary<(string cardId, string retailerName), Product>();

      foreach (var priceResponse in priceResponses)
      {
        foreach (var product in priceResponse.Products)
        {
          var key = (priceResponse.Card.Name, product.Retailer_Name);
          if (!cheapestProducts.ContainsKey(key) || (product.Price.HasValue && product.Price < cheapestProducts[key].Price) && ShouldShowProduct(product))
          {
            cheapestProducts[key] = product;
          }
        }
      }

      // Extract unique retailer names and card names
      var retailerNames = cheapestProducts.Values.Select(p => p.Retailer_Name).Distinct().ToList();
      var cardNames = priceResponses.Select(pr => pr.Card.Name).Distinct().ToList();

      // Create the CSV content
      var csvLines = new List<string>();

      // Header row with retailer names
      var headerRow = "," + string.Join(",", retailerNames);
      csvLines.Add(headerRow);

      // Rows with card names and prices
      foreach (var cardName in cardNames)
      {
        var row = new List<string> { cardName };

        foreach (var retailerName in retailerNames)
        {
          var key = (cardName, retailerName);
          if (cheapestProducts.TryGetValue(key, out var product) && product.Price.HasValue)
          {
            row.Add((product.Price.Value / 100).ToString());
          }
          else
          {
            row.Add(string.Empty);
          }
        }

        csvLines.Add(string.Join(",", row));
      }

      csvLines.Add(string.Empty);

      // Final row with total prices for each column
      var totalRow = new List<string> { "Total Price" };
      foreach (var retailerName in retailerNames)
      {
        var totalPrice = cheapestProducts
            .Where(cp => cp.Key.retailerName == retailerName && cp.Value.Price.HasValue)
            .Sum(cp => cp.Value.Price.Value);
        totalRow.Add((totalPrice / 100).ToString());
      }
      csvLines.Add(string.Join(",", totalRow));

      var fileName = DateTime.Now.ToFileTime() + ".csv";

      File.WriteAllLines(fileName, csvLines);

      var csvData = File.ReadAllLines(fileName);

      // Parse CSV data and load into DataGridView
      LoadCsvDataIntoDataGridView(csvData);
    }

    private void LoadCsvDataIntoDataGridView(string[] csvData)
    {
      // Clear existing DataGridView content
      dataGridView1.Rows.Clear();
      dataGridView1.Columns.Clear();

      // Assuming the first row contains headers, split each row by comma to get columns
      var rows = csvData.Select(line => line.Split(',')).ToArray();

      // Set headers from the first row
      //dataGridView1.ColumnCount = rows.Max(r => r.Length);
      for (var i = 0; i < rows[0].Length; i++)
      {
        dataGridView1.Columns.Add($"Column{i}", rows[0][i]); // Use index if headers not available
      }

      // Add rows from the CSV data (skip the first row as it's headers)
      for (var i = 1; i < rows.Length; i++)
      {
        dataGridView1.Rows.Add(rows[i]);
      }
    }

    //AL.
    //TODO - seems broken cos art card for dreamtide whale is still appearing.
    //Use this to hide things like art cards
    private bool ShouldShowProduct(Product product)
      => BlackListTerms.All(blackListTerm => !product.Name.ToLower().Contains(blackListTerm) && product.Price is > 0);

    private void btn_save_Click(object sender, EventArgs e)
    {
      //AL.
      //TODO
      //var sfd = new SaveFileDialog();
      //sfd.Filter = "Comma-Separated Values File(*.csv)|*.csv";
      //sfd.ShowDialog();
      MessageBox.Show("Feature not yet implemented", "Sorry");
    }
  }
}
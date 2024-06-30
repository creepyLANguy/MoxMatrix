using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

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

    const string buttonDefault = @"Query Prices";
    const string processingText = @" - Processing...";
    const string queryingText = @"Querying prices from each store...";

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
      public bool Is_Foil { get; set; }
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

    private readonly char csvDelim = ';';
    private readonly string[] BlackListTerms = { "art card" };

    private readonly string urlHeadingTag_Open = "-- ";
    private readonly string urlHeadingTag_Close = "  --";

    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      btn_go.Text = buttonDefault;

      dataGridView1.Columns.Add($"Blank", "Results will appear here...");
      SetDoubleBuffer(dataGridView1, true);
      DoubleBuffered = true;

      //AL.
      //DEBUG
      inputBox.Text += @"mox t" + Environment.NewLine;
      inputBox.Text += @"opportunis" + Environment.NewLine;
      inputBox.Text += @"countersp" + Environment.NewLine;
      inputBox.Text += @"dreamtide" + Environment.NewLine;
      inputBox.Text += @"somerandomstring" + Environment.NewLine;
      inputBox.Text += @"asmora" + Environment.NewLine;
      inputBox.Text += @"esper sen" + Environment.NewLine;
      inputBox.Text += @"clara" + Environment.NewLine;
      inputBox.Text += @"the ur" + Environment.NewLine;
      inputBox.Text += @"teferi, master" + Environment.NewLine;
      //
    }

    private async void btn_go_Click(object sender, EventArgs e)
    {
      Text += processingText;
      btn_go.Text = queryingText;
      dataGridView1.Visible = false;
      Enabled = false;

      txt_unknownCards.Text = string.Empty;
      txt_outOfStock.Text = string.Empty;
      txt_storesSummaries.Text = string.Empty;
      txt_urls.Text = string.Empty;

      try
      {
        await DoTheThings();
      }
      catch (Exception ex)
      {
        MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      Text = Text.Replace(processingText, string.Empty);
      btn_go.Text = buttonDefault;
      dataGridView1.Visible = true;
      Enabled = true;
    }

    private async Task DoTheThings()
    {
      var cardMatches = await GetCardMatchesListAsync();

      var priceList = await GetPriceListAsync(cardMatches);
      if (btn_foils.Checked)
      {
        foreach (var priceGroup in priceList)
        {
          var foils = new List<Product>();
          foreach (var product in priceGroup.Products)
          {
            if (IsMostLikelyFoil(product))
            {
              foils.Add(product);
            }
          }
          priceGroup.Products = foils;
        }
      }

      foreach (var priceGroup in priceList)
      {
        if (priceGroup.Products.Count == 0)
        {
          txt_outOfStock.Text += priceGroup.Card.Name + Environment.NewLine;
        }
      }

      var fileName = DateTime.Now.ToFileTime() + ".csv";

      GenerateCsv(priceList, fileName);

      var csvData = await File.ReadAllLinesAsync(fileName);
      LoadCsvDataIntoDataGridView(csvData);

      var summaries = new List<Tuple<Tuple<int, int>, string>>();

      for (var col = 1; col < dataGridView1.Columns.Count; col++)
      {
        var stock = 0;
        for (var row = 0; row < dataGridView1.Rows.Count - 1; row++)
        {
          if (dataGridView1.Rows[row].Cells[col].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[row].Cells[col].Value.ToString()))
          {
            ++stock;
          }
        }

        var storeName = dataGridView1.Columns[col].HeaderText;
        if (dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[col].Value == null)
        {
          return;
        }
        var totalCost = int.Parse(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[col].Value.ToString());

        var summary =
          storeName + Environment.NewLine +
          "In Stock : " + stock + Environment.NewLine +
          "Total : R" + totalCost + Environment.NewLine +
          Environment.NewLine;

        summaries.Add(new Tuple<Tuple<int, int>, string>(new Tuple<int, int>(stock, totalCost), summary));
      }

      var sortedSummaries = summaries.OrderByDescending(o => o.Item1.Item1).ThenBy(o => o.Item1.Item2).ToList();

      foreach (var c in sortedSummaries)
      {
        txt_storesSummaries.Text += c.Item2;
      }


      ReorderColumns(sortedSummaries);
    }

    private static bool IsMostLikelyFoil(Product product) =>
      product.Is_Foil || product.Name.ToLower().Contains("foil");

    public async Task<List<Card>> GetCardMatchesListAsync()
    {
      var cardsInput = inputBox.Text.Split(Environment.NewLine).Where(it => it.Length > 0);
      var tasks = cardsInput.Select(GetCardMatchAsync).ToList();

      var results = await Task.WhenAll(tasks);

      return results.OfType<Card>().ToList();
    }

    private async Task<Card?> GetCardMatchAsync(string input)
    {
      using var httpClient = new HttpClient();
      var uri = CardMatchEndpoint + input;
      var response = await httpClient.GetAsync(uri);
      var results = await response.Content.ReadAsStringAsync();
      var cardsResponse = JsonConvert.DeserializeObject<CardsResponse>(results);

      if (cardsResponse == null || cardsResponse.Cards.Count == 0)
      {
        txt_unknownCards.Text += input + Environment.NewLine;
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

    private void GenerateCsv(List<PriceResponse> priceResponses, string fileName)
    {
      var cheapestProducts = new Dictionary<(string cardId, string retailerName), Product>();

      foreach (var priceResponse in priceResponses)
      {
        foreach (var product in priceResponse.Products)
        {
          var key = (priceResponse.Card.Name, product.Retailer_Name);
          if (!cheapestProducts.ContainsKey(key) || (product.Price.HasValue && product.Price < cheapestProducts[key].Price))
          {
            if (ShouldShowProduct(product))
            {
              cheapestProducts[key] = product;
            }
          }
        }
      }

      // Extract unique retailer names and card names
      var retailerNames = cheapestProducts.Values.Select(p => p.Retailer_Name).Distinct().ToList();
      var cardNames = priceResponses.Select(pr => pr.Card.Name).Distinct().ToList();

      foreach (var cardName in cardNames)
      {
        var heading = urlHeadingTag_Open + cardName + urlHeadingTag_Close + Environment.NewLine + Environment.NewLine;

        var buffer = heading;

        foreach (var priceResponse in priceResponses)
        {
          if (priceResponse.Card.Name == cardName)
          {
            if (priceResponse.Products.Count == 0)
            {
              continue;
            }

            foreach (var product in priceResponse.Products.OrderBy(p => p.Price))
            {
              if (product.Price.HasValue == false)
              {
                continue;
              }

              buffer += "R" + product.Price / 100 + Environment.NewLine;
              buffer += product.Name + Environment.NewLine;
              buffer += product.Retailer_Name + Environment.NewLine;
              buffer += product.Link + Environment.NewLine;
              buffer += Environment.NewLine;
            }
          }
        }

        if (buffer == heading)
        {
          continue;
        }

        buffer += "___";
        buffer += Environment.NewLine + Environment.NewLine;
        txt_urls.Text += buffer;
      }

      FormatUrlsTextBox();

      // Create the CSV content
      var csvLines = new List<string>();

      // Header row with retailer names
      var headerRow = csvDelim + string.Join(csvDelim, retailerNames);
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
            row.Add((product.Price.Value / 100).ToString() + (IsMostLikelyFoil(product) ? "  ✨" : ""));
          }
          else
          {
            row.Add(string.Empty);
          }
        }

        csvLines.Add(string.Join(csvDelim, row));
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
      csvLines.Add(string.Join(csvDelim, totalRow));

      File.WriteAllLines(fileName, csvLines);
    }

    private void FormatUrlsTextBox()
    {
      var openingTagLocations = new List<int>();
      var input1 = txt_urls.Text;
      var index1 = input1.IndexOf(urlHeadingTag_Open);
      while (index1 != -1)
      {
        openingTagLocations.Add(index1);
        index1 = input1.IndexOf(urlHeadingTag_Open, index1 + urlHeadingTag_Open.Length);
      }

      var closingTagLocations = new List<int>();
      var input2 = txt_urls.Text;
      var index2 = input2.IndexOf(urlHeadingTag_Close);
      while (index2 != -1)
      {
        closingTagLocations.Add(index2);
        index2 = input2.IndexOf(urlHeadingTag_Close, index2 + urlHeadingTag_Close.Length);
      }

      for (var i = 0; i < Math.Min(openingTagLocations.Count, closingTagLocations.Count); ++i)
      {
        var startIndex = openingTagLocations[i];
        var length = closingTagLocations[i] + urlHeadingTag_Close.Length - startIndex;

        txt_urls.SelectionStart = startIndex;
        txt_urls.SelectionLength = length;
        txt_urls.SelectionFont = new Font(txt_urls.Font, FontStyle.Bold | FontStyle.Underline);
      }
    }

    private void LoadCsvDataIntoDataGridView(string[] csvData)
    {
      // Clear existing DataGridView content
      dataGridView1.Rows.Clear();
      dataGridView1.Columns.Clear();

      // Assuming the first row contains headers, split each row by delim to get columns
      var rows = csvData.Select(line => line.Split(csvDelim)).ToArray();

      // Set headers from the first row
      for (var i = 0; i < rows[0].Length; i++)
      {
        dataGridView1.Columns.Add($"Column{i}", rows[0][i]);
      }

      // Add rows from the CSV data (skip the first row as it's headers)
      for (var i = 1; i < rows.Length; i++)
      {
        dataGridView1.Rows.Add(rows[i]);
      }
    }

    private void ReorderColumns(List<Tuple<Tuple<int, int>, string>> sortedSummaries)
    {
      dataGridView1.Columns[0].DisplayIndex = 0;

      for (var i = 1; i < dataGridView1.Columns.Count; ++i)
      {
        var storeName = dataGridView1.Columns[i].HeaderText;
        var matchingSummary = sortedSummaries.Where(s => s.Item2.Contains(storeName)).First();
        var displayIndex = sortedSummaries.IndexOf(matchingSummary);
        dataGridView1.Columns[i].DisplayIndex = displayIndex + 1;
      }
    }

    private bool ShouldShowProduct(Product product)
      => BlackListTerms.All(blackListTerm => !product.Name.ToLower().Contains(blackListTerm.ToLower()) && product.Price is > 0);

    private void btn_exportCSV_Click(object sender, EventArgs e)
    {
      if (dataGridView1.Rows.Count == 0)
      {
        MessageBox.Show("No data to export.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      using var saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
      saveFileDialog.Title = "Save as CSV file";
      saveFileDialog.FileName = "MoxMatrix_Export.csv";

      if (saveFileDialog.ShowDialog() == DialogResult.OK)
      {
        var fileName = saveFileDialog.FileName;

        ExportDataGridViewToCSV(fileName);

        OpenFile(fileName);
      }
    }

    public void ExportDataGridViewToCSV(string filePath)
    {
      var csvContent = new StringBuilder();

      // Get headers
      foreach (DataGridViewColumn column in dataGridView1.Columns)
      {
        csvContent.Append(column.HeaderText + csvDelim);
      }
      csvContent.Remove(csvContent.Length - 1, 1); // Remove last comma
      csvContent.AppendLine();

      // Get rows
      foreach (DataGridViewRow row in dataGridView1.Rows)
      {
        if (row.IsNewRow)
        {
          continue; // Skip the new row placeholder
        }

        foreach (DataGridViewCell cell in row.Cells)
        {
          csvContent.Append(cell.Value + "" + csvDelim);
        }
        csvContent.Remove(csvContent.Length - 1, 1); // Remove last comma
        csvContent.AppendLine();
      }

      // Write to file
      File.WriteAllText(filePath, csvContent.ToString(), Encoding.UTF8);
    }

    void OpenFile(string fileName)
    {
      try
      {
        Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void Form1_ResizeBegin(object sender, EventArgs e)
    {
      //dataGridView1.Visible = false;
    }

    private void Form1_ResizeEnd(object sender, EventArgs e)
    {
      //dataGridView1.Visible = true;
      splitContainer1.Invalidate();
    }

    private void splitContainer1_Paint(object sender, PaintEventArgs e)
    {
      var dotSize = 4;

      //var control = sender as SplitContainer;
      var control = splitContainer1;

      //paint the three dots'
      var points = new Point[3];
      var w = control.Width;
      var h = control.Height;
      var d = control.SplitterDistance;
      var sW = control.SplitterWidth;

      //calculate the position of the points'
      if (control.Orientation == Orientation.Horizontal)
      {
        points[0] = new Point((w / 2), d + (sW / 2));
        points[1] = new Point(points[0].X - 10, points[0].Y);
        points[2] = new Point(points[0].X + 10, points[0].Y);
      }
      else
      {
        points[0] = new Point(d + (sW / 2), (h / 2));
        points[1] = new Point(points[0].X, points[0].Y - 10);
        points[2] = new Point(points[0].X, points[0].Y + 10);
      }

      foreach (var p in points)
      {
        p.Offset(-2, -2);
        e.Graphics.FillEllipse(SystemBrushes.ControlDark,
          new(p, new Size(dotSize * 3, dotSize / 2)));
        //new(p, new Size(dotSize, dotSize)));

        p.Offset(1, 1);
        e.Graphics.FillEllipse(SystemBrushes.ControlLight,
          new Rectangle(p, new Size(dotSize * 3, dotSize / 2)));
        //new Rectangle(p, new Size(dotSize, dotSize)));
      }
    }

    private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
    {
      if (e.RowIndex % 2 == 0)
      {
        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.AliceBlue;
      }
      else
      {
        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
      }
    }

    static void SetDoubleBuffer(Control dgv, bool DoubleBuffered)
    {
      typeof(Control).InvokeMember("DoubleBuffered",
          BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
          null, dgv, new object[] { DoubleBuffered });
    }

    private void btn_saveUrls_Click(object sender, EventArgs e)
    {
      if (txt_urls.Text.Length == 0)
      {
        MessageBox.Show("No data to export.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      using var saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Plain Text files (*.txt)|*.txt";
      saveFileDialog.Title = "Save as Plain Text file";
      saveFileDialog.FileName = "MoxMatrix_URLs.txt";

      if (saveFileDialog.ShowDialog() == DialogResult.OK)
      {
        var fileName = saveFileDialog.FileName;

        File.WriteAllText(fileName, txt_urls.Text, Encoding.UTF8);

        OpenFile(fileName);
      }
    }
  }
}
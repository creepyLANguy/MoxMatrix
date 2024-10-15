using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using MoxMatrix.Properties;
using System.Net.Mime;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
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

    class Vendor
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Type { get; set; }
    }

    const string loadingVendorsText = @"Loading Vendors...";

    const string buttonDefault = @"Query Prices";
    const string processingText = @" - Processing...";
    const string queryingText = @"Querying prices from each store...";

    private const string BaseUrl = "https://moxmonolith.com";

    private const string CardMatchEndpoint = "/card/search?name=";

    private const string PricesEndpoint = "/card/{id}/products";

    private const string ImageEndpoint = "/named?exact=";

    private const string VendorsEndpoint = "/vendors";

    private List<Vendor> _vendorsList;

    private readonly string _queryOutputFolderName = "queries";

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

    private Dictionary<string, string> imageUrls;

    private ImageForm imageForm;

    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      SetupImageForm();

      btn_go.Text = buttonDefault;

      dataGridView1.Columns.Add("Blank", "Results will appear here...");
      SetDoubleBuffer(dataGridView1, true);
      DoubleBuffered = true;

#if DEBUG
      PopulateDebugData();
#endif

      SuspendForm(loadingVendorsText);

      GetVendors();

      UnsuspendForm(loadingVendorsText);
    }

    private void SetupImageForm()
    {
      imageForm = new();
      imageForm.Icon = Resources.icon;
      imageForm.Text = "";
      imageForm.BackColor = Color.LimeGreen;
      imageForm.TransparencyKey = Color.LimeGreen;
      imageForm.FormBorderStyle = FormBorderStyle.None;
      imageForm.Size = new Size(250, 350);
      imageForm.TopLevel = true;
      imageForm.TopMost = true;
      imageForm.ShowInTaskbar = false;
      imageForm.Visible = false;
    }

    private void PopulateDebugData()
    {
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
    }

    private void SuspendForm(string text)
    {
      Text += text;
      btn_go.Text = queryingText;
      dataGridView1.Visible = false;
      Enabled = false;
      Cursor.Current = Cursors.WaitCursor;
    }

    private void UnsuspendForm(string text)
    {
      Text = Text.Replace(text, string.Empty);
      btn_go.Text = buttonDefault;
      dataGridView1.Visible = true;
      Enabled = true;
      Cursor.Current = Cursors.Default;
    }

    private void GetVendors()
    {
      using var httpClient = new HttpClient();
      var uri = BaseUrl + VendorsEndpoint;
      var request = new HttpRequestMessage(HttpMethod.Get, uri);

      var response = httpClient.Send(request);

      if (response.StatusCode != HttpStatusCode.OK)
      {
        MessageBox.Show(
          "Failed to retrieve a list of vendors.\n\nThe application will now close.",
          "Mox Matrix (beta) - ERROR",
          MessageBoxButtons.OK,
          MessageBoxIcon.Error);

        Close();
        return;
      }

      var html = response.Content.ReadAsStringAsync().Result;
      var doc = new HtmlDocument();
      doc.LoadHtml(html);

      var dataPage = doc.DocumentNode
        .SelectSingleNode("//div[@id='app']")?
        .GetAttributeValue("data-page", "");

      dataPage = dataPage.Replace("&quot", "");
      dataPage = dataPage.Replace(";", "");

      var lines = dataPage.Split("},");

      _vendorsList = new List<Vendor>();

      foreach (var line in lines)
      {
        if (line.Contains("id") == false)
        {
          continue;
        }

        var props = line.Split(",");

        var id = props.First(i => i.Contains("id:"));
        id = id.Substring(id.LastIndexOf(":") + 1);

        var name = props.First(i => i.Contains("name:"));
        name = name.Substring(name.LastIndexOf(":") + 1);

        var type = props.First(i => i.Contains("type:"));
        type = type.Substring(type.LastIndexOf(":") + 1);

        var v = new Vendor
        {
          Id = int.Parse(id),
          Name = name,
          Type = type
        };
        _vendorsList.Add(v);
      }

      foreach (var vendor in _vendorsList)
      {
        if (vendor.Type == "business")
        {
          cl_businesses.Items.Add(vendor.Name, true);
        }
        else if (vendor.Type == "individual")
        {
          cl_individuals.Items.Add(vendor.Name, true);
        }
      }
    }

    private async void btn_go_Click(object sender, EventArgs e)
    {
      if (cl_individuals.CheckedItems.Count == 0 && cl_businesses.CheckedItems.Count == 0)
      {
        MessageBox.Show("No vendors selected!", "Mox Matrix (beta) - ERROR");
        return;
      }

      SuspendForm(processingText);

      txt_unknownCards.Text = string.Empty;
      txt_outOfStock.Text = string.Empty;
      txt_storesSummaries.Text = string.Empty;
      txt_urls.Text = string.Empty;
      txt_errorFetching.Text = string.Empty;

      try
      {
        await DoTheThings();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      UnsuspendForm(processingText);
    }

    private async Task DoTheThings()
    {
      RemoveDuplicates(inputBox);

      var cardMatches = await GetCardMatchesListAsync();

      var priceList = new List<PriceResponse>();
      try
      {
        priceList = await GetPriceListAsync(cardMatches);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

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

      imageUrls = new();
      foreach (var priceResponse in priceList)
      {
        var cardName = priceResponse.Card.Name;

        //using var httpClient = new HttpClient();
        //var uri = BaseUrl + ImageEndpoint + Uri.EscapeDataString(cardName);
        //var response = await httpClient.GetAsync(uri);

        //if (!response.IsSuccessStatusCode)
        //{
        //  continue;
        //}

        //var jsonResponse = await response.Content.ReadAsStringAsync();
        //var data = JObject.Parse(jsonResponse);
        //var url = data["image_uris"]?["border_crop"]?.ToString();

        //AL.
        //TODO - FIX!
        if (priceResponse.Products.Count == 0)
        {
          continue;
        }

        var url = priceResponse.Products.First().Image;
        if (priceResponse.Products.Count > 2)
        {
          url = priceResponse.Products[2].Image;
        }
        //

        imageUrls.Add(cardName, url);
      }

      foreach (var priceGroup in priceList)
      {
        if (priceGroup.Products.Count == 0)
        {
          txt_outOfStock.Text += priceGroup.Card.Name + Environment.NewLine;
        }
      }

      var csvLines = GenerateCsv(priceList);

      var queryOutputFolderFullPath =
        Path.Join(Path.GetDirectoryName(AppContext.BaseDirectory), _queryOutputFolderName);

      Directory.CreateDirectory(queryOutputFolderFullPath);

      var fullFilePath = Path.Join(queryOutputFolderFullPath, DateTime.Now.ToFileTime() + ".csv");
      File.WriteAllLines(fullFilePath, csvLines);

      LoadCsvDataIntoDataGridView(ref csvLines);

      var summaries = new List<Tuple<Tuple<int, int>, string>>();

      for (var col = 1; col < dataGridView1.Columns.Count; col++)
      {
        var stock = 0;
        for (var row = 0; row < dataGridView1.Rows.Count - 1; row++)
        {
          if (dataGridView1.Rows[row].Cells[col].Value != null &&
              !string.IsNullOrEmpty(dataGridView1.Rows[row].Cells[col].Value.ToString()))
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

      //Doing this twice cos... datagridviews are wierd about how they reorder columns. 
      ReorderColumns(sortedSummaries);
      ReorderColumns(sortedSummaries);

      ReorderRows();
    }

    private void RemoveDuplicates(Control control)
      => control.Text = string.Join(Environment.NewLine, control.Text.Split(Environment.NewLine).Distinct());

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
      var uri = BaseUrl + CardMatchEndpoint + input;
      var response = await httpClient.GetAsync(uri);

      if (response.StatusCode != HttpStatusCode.OK)
      {
        txt_errorFetching.Text += input + Environment.NewLine;
        return null;
      }

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

    private async Task<PriceResponse?> GetPriceAsync(Card cardMatch)
    {
      using var httpClient = new HttpClient();
      var uri = BaseUrl + PricesEndpoint.Replace("{id}", cardMatch.Id) + GetVendorsQueryString();
      var response = await httpClient.GetAsync(uri);
      if (response.StatusCode != HttpStatusCode.OK)
      {
        txt_errorFetching.Text += cardMatch.Name + Environment.NewLine;
        return null;
      }
      var results = await response.Content.ReadAsStringAsync();

      var contentType = response.Content.Headers.ContentType;
      if (contentType == null || contentType.MediaType != MediaTypeNames.Application.Json)
      {
        throw new Exception("Something is wrong with the response.\nA retailer ID might be invalid in the request params.");
      }

      return JsonConvert.DeserializeObject<PriceResponse>(results);
    }

    private string GetVendorsQueryString()
    {
      var buffRevised = "?retailers[]=";

      //AL.
      //TODO - extract to common function. Or don't. All of the code is rubbish anyway.
      foreach (var checkedItem in cl_businesses.CheckedItems)
      {
        var vendorId = _vendorsList.First(v => v.Name == checkedItem).Id;
        buffRevised += vendorId + "&retailers[]=";
      }


      foreach (var checkedItem in cl_individuals.CheckedItems)
      {
        var vendorId = _vendorsList.First(v => v.Name == checkedItem).Id;
        buffRevised += vendorId + "&retailers[]=";
      }
      
      if (buffRevised.EndsWith("=")) 
      {
        buffRevised = buffRevised[..buffRevised.LastIndexOf('&')];
      }

      return buffRevised;
    }
    private List<string> GenerateCsv(List<PriceResponse> priceResponses)
    {
      var cheapestProducts = new Dictionary<(string cardId, string vendorName), Product>();

      foreach (var priceResponse in priceResponses)
      {
        foreach (var product in priceResponse.Products)
        {
          var key = (priceResponse.Card.Name, product.Retailer_Name);
          if (!cheapestProducts.ContainsKey(key) ||
              (product.Price.HasValue && product.Price < cheapestProducts[key].Price))
          {
            if (ShouldShowProduct(product))
            {
              cheapestProducts[key] = product;
            }
          }
        }
      }

      // Extract unique vendor names and card names
      var vendorNames = cheapestProducts.Values.Select(p => p.Retailer_Name).Distinct().ToList();
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

      // Header row with vendor names
      var headerRow = csvDelim + string.Join(csvDelim, vendorNames);
      csvLines.Add(headerRow);

      // Rows with card names and prices
      foreach (var cardName in cardNames)
      {
        var row = new List<string> { cardName };

        foreach (var vendorName in vendorNames)
        {
          var key = (cardName, vendorName);
          if (cheapestProducts.TryGetValue(key, out var product) && product.Price.HasValue)
          {
            row.Add((product.Price.Value / 100) + (IsMostLikelyFoil(product) ? "  ✨" : ""));
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
      foreach (var vendorName in vendorNames)
      {
        var totalPrice = cheapestProducts
          .Where(cp => cp.Key.vendorName == vendorName && cp.Value.Price.HasValue)
          .Sum(cp => cp.Value.Price.Value);
        totalRow.Add((totalPrice / 100).ToString());
      }

      csvLines.Add(string.Join(csvDelim, totalRow));

      return csvLines;
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

    private void LoadCsvDataIntoDataGridView(ref List<string> csvData)
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
        var matchingSummary = sortedSummaries.First(s => s.Item2.Contains(storeName));
        var displayIndex = sortedSummaries.IndexOf(matchingSummary);
        dataGridView1.Columns[i].DisplayIndex = displayIndex + 1;
      }
    }

    private int CountNonBlankStrings(DataGridViewRow row)
    {
      var count = 0;
      for (var index = 0; index < row.Cells.Count; index++)
      {
        var cell = row.Cells[index];
        if (cell.Value != null && !string.IsNullOrWhiteSpace(cell.Value.ToString()))
        {
          count++;
        }
      }

      return count;
    }

    private void ReorderRows()
    {
      // Check if there are rows to sort
      if (dataGridView1.Rows.Count <= 1) return;

      // Separate the last row
      var lastRow = dataGridView1.Rows[dataGridView1.Rows.Count - 1];

      // Create a list of rows excluding the last one (if it's not the new row)
      var rows = dataGridView1.Rows.Cast<DataGridViewRow>()
        .Take(dataGridView1.Rows.Count - 2)
        .ToList();

      // Sort the rows
      rows.Sort((row1, row2) => CountNonBlankStrings(row2).CompareTo(CountNonBlankStrings(row1)));

      // Clear the DataGridView and add sorted rows back
      dataGridView1.Rows.Clear();
      foreach (var row in rows)
      {
        dataGridView1.Rows.Add(row);
      }

      dataGridView1.Rows.Add();
      dataGridView1.Rows.Add(lastRow);
    }

    private bool ShouldShowProduct(Product product)
      => BlackListTerms.All(blackListTerm =>
        !product.Name.ToLower().Contains(blackListTerm.ToLower()) && product.Price is > 0);

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

    private void txt_urls_LinkClicked(object sender, LinkClickedEventArgs e)
    {
      var args = "/C start " + e.LinkText.Replace("&", "^&");

      var psi = new ProcessStartInfo
      {
        FileName = "cmd",
        WindowStyle = ProcessWindowStyle.Hidden,
        UseShellExecute = false,
        CreateNoWindow = true,
        Arguments = args
      };
      Process.Start(psi);
    }


    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
      => FocusOnCorrespondingURL(e);


    private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
    {
      FocusOnCorrespondingURL(e);

      if (dataGridView1.Focused == false)
      {
        imageForm.Visible = false;
        return;
      }

      var cellValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
      if (cellValue == null)
      {
        imageForm.Visible = false;
        return;
      }

      if (imageUrls == null || !imageUrls.TryGetValue(cellValue.ToString(), out var url))
      {
        imageForm.Visible = false;
        return;
      }

      //Do this cos the grid selects the first cell as soon as it's populated and so shows the image window automatically.
      if ((string)imageForm.Tag == null)
      {
        imageForm.Visible = true;
        imageForm.Visible = false;
        imageForm.Tag = url;
        return;
      }

      imageForm.Visible = false;

      var rect = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
      var point = dataGridView1.PointToScreen(rect.Location);
      var pos = new Point(point.X, point.Y + rect.Height);
      imageForm.Location = pos;

      imageForm.Tag = url;
      var result = imageForm.SetPicture(url);
      imageForm.Visible = result;

      Focus();
    }

    private void FocusOnCorrespondingURL(DataGridViewCellEventArgs e)
    {
      if (e.RowIndex < 0)
      {
        return;
      }

      if (dataGridView1.Rows[e.RowIndex].Cells[0].Value == null)
      {
        return;
      }

      var target = urlHeadingTag_Open + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + urlHeadingTag_Close;
      var index = txt_urls.Text.IndexOf(target);
      if (index == -1)
      {
        return;
      }

      if (txt_urls.SelectionStart == index)
      {
        return;
      }

      txt_urls.SelectionStart = index;
      txt_urls.ScrollToCaret();
    }

    private void inputBox_DragDrop(object sender, DragEventArgs e)
    {
      var buffer = "";

      if (inputBox.Text.Trim().Length > 0)
      {
        var choice =
          MessageBox.Show("Would you like to replace the card list entries?\n('No' will append to the existing list)",
            "", MessageBoxButtons.YesNoCancel);
        if (choice == DialogResult.No)
        {
          buffer = inputBox.Text;
        }
        else if (choice == DialogResult.Cancel)
        {
          return;
        }
      }

      if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
      {
        var files = (string[])e.Data.GetData(DataFormats.FileDrop);
        if (files == null || files.Length == 0)
        {
          return;
        }

        foreach (var file in files)
        {
          //Note : StreamReader.ReadToEnd()is faster. 
          var fileContents = File.ReadAllText(file);
          buffer += fileContents;
          buffer += Environment.NewLine + Environment.NewLine;
        }
      }

      else if (e.Data.GetDataPresent(DataFormats.Text, true))
      {
        buffer += e.Data.GetData(DataFormats.Text, true);
      }

      if (buffer.Length > 0)
      {
        inputBox.Text = buffer;
      }
    }

    private void inputBox_DragOver(object sender, DragEventArgs e)
    {
      if (
        (e.Data.GetDataPresent(DataFormats.FileDrop, true)) ||
        (e.Data.GetDataPresent(DataFormats.Text, true))
      )
      {
        e.Effect = DragDropEffects.All;
      }
      else
      {
        e.Effect = DragDropEffects.None;
      }
    }

    private void dataGridView1_Leave(object sender, EventArgs e)
    {
      imageForm.Visible = false;
    }

    private void Form1_LocationChanged(object sender, EventArgs e)
    {
      if (imageForm != null)
      {
        imageForm.Visible = false;
      }
    }

    private void Form1_ClientSizeChanged(object sender, EventArgs e)
    {
      if (imageForm != null)
      {
        imageForm.Visible = false;
      }
    }

    private void Form1_RegionChanged(object sender, EventArgs e)
    {
      if (imageForm != null)
      {
        imageForm.Visible = false;
      }
    }

    private void cl_businesses_MouseUp(object sender, MouseEventArgs e)
    {
      cb_businessesAll.CheckState =
        cl_businesses.CheckedItems.Count == cl_businesses.Items.Count ?
          CheckState.Checked : CheckState.Indeterminate;

      if (cb_businessesAll.CheckState == CheckState.Indeterminate && cl_businesses.CheckedItems.Count == 0)
      {
        cb_businessesAll.CheckState = CheckState.Unchecked;
      }
    }

    private void cl_businesses_KeyUp(object sender, KeyEventArgs e)
      => cl_businesses_MouseUp(sender, null);

    private void cl_individuals_MouseUp(object sender, MouseEventArgs e)
    {
      cb_individualsAll.CheckState =
        cl_individuals.CheckedItems.Count == cl_individuals.Items.Count ?
          CheckState.Checked : CheckState.Indeterminate;

      if (cb_individualsAll.CheckState == CheckState.Indeterminate && cl_individuals.CheckedItems.Count == 0)
      {
        cb_individualsAll.CheckState = CheckState.Unchecked;
      }
    }

    private void cl_individuals_KeyUp(object sender, KeyEventArgs e)
      => cl_individuals_MouseUp(sender, null);

    private void cb_businessesAll_CheckedChanged(object sender, EventArgs e)
    {
      if (cb_businessesAll.CheckState == CheckState.Indeterminate)
      {
        return;
      }

      for (var i = 0; i < cl_businesses.Items.Count; i++)
      {
        cl_businesses.SetItemChecked(i, cb_businessesAll.CheckState == CheckState.Checked);
      }
    }

    private void cb_individualsAll_CheckedChanged(object sender, EventArgs e)
    {
      if (cb_individualsAll.CheckState == CheckState.Indeterminate)
      {
        return;
      }

      for (var i = 0; i < cl_individuals.Items.Count; i++)
      {
        cl_individuals.SetItemChecked(i, cb_individualsAll.CheckState == CheckState.Checked);
      }
    }
  }
}
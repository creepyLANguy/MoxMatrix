namespace MoxMatrix;

public class ImageCache
{
  private readonly Dictionary<string, (Image Image, LinkedListNode<string> Node)> _imageDictionary = new();
  private readonly LinkedList<string> _usageOrder = new();
  private const int MaxCachedImages = 100;

  private readonly HttpClient _httpClient = new();
  private const string ImageEndpoint = "https://api.scryfall.com/cards/named?exact=";
  private const string ImageParams = "&format=image&version=normal";

  private readonly Image _brokenImagePlaceholder = GenerateBrokenImagePlaceholder();

  
  private void MoveToFront(string cardName)
  {
    var node = _imageDictionary[cardName].Node;
    _usageOrder.Remove(node);
    _usageOrder.AddFirst(node);
  }

  private void Add(string cardName, Image image)
  {
    if (_imageDictionary.Count >= MaxCachedImages)
    {
      var leastUsedCardName = _usageOrder.Last!.Value;
      _usageOrder.RemoveLast();
      _imageDictionary.Remove(leastUsedCardName);
    }

    var node = _usageOrder.AddFirst(cardName);
    _imageDictionary[cardName] = (image, node);
  }

  public Image Get(string cardName)
  {
    if (_imageDictionary.TryGetValue(cardName, out var cachedEntry))
    {
      MoveToFront(cardName);
      return cachedEntry.Image;
    }

    var downloadedImage = DownloadImage(cardName);

    Add(cardName, downloadedImage);

    return downloadedImage;
  }

  //TODO - implement a way to stop/restart this thread (when the form is closed, when the cardNames list is updated, on exceptions and failures, etc).
  public void StartCachingThread(List<string> cardNames)
  {
    foreach (var cardName in cardNames)
    {
      if (_imageDictionary.ContainsKey(cardName))
      {
        continue;
      }

      var downloadedImage = DownloadImage(cardName);

      Add(cardName, downloadedImage);
    }
  }

  private Image DownloadImage(string cardName)
  {
    //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

    var imageUrl = ImageEndpoint + Uri.EscapeDataString(cardName) + ImageParams;

    var request = new HttpRequestMessage(HttpMethod.Get, imageUrl);
    request.Headers.Add("User-Agent", "YourAppName/1.0 (your-email@example.com)");
    request.Headers.Add("Accept", "application/json;q=0.9,*/*;q=0.8");

    var response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
    if (response.IsSuccessStatusCode == false)
    {
      return _brokenImagePlaceholder;
    }

    using var imageStream = response.Content.ReadAsStream();
    return Image.FromStream(imageStream);
  }

  private static Image GenerateBrokenImagePlaceholder()
  {
    const int width = 1000;
    const int height = 1000;
    const int fontSize = 78;
    const float multiplier = 1.30f;
    const int linePenWidth = 2;
    var strokeColour = Color.Gray;
    var backgroundColour = Color.White;
    var linePen = new Pen(strokeColour, linePenWidth);
    var rectPen = new Pen(backgroundColour, fontSize * multiplier);
    var stringBrush = new SolidBrush(strokeColour);
    var font = new Font(FontFamily.GenericMonospace, fontSize);

    var bmp = new Bitmap(width, height);
    using var g = Graphics.FromImage(bmp);
    g.Clear(backgroundColour);

    g.DrawLine(linePen, 0, 0, width, height);
    g.DrawLine(linePen, width, 0, 0, height);
    g.DrawRectangle(rectPen, 0, height / 2f - fontSize / 2f, width, fontSize * multiplier);
    g.DrawString("Image Not Found", font, stringBrush, new PointF(0, height / 2f - fontSize / 2f));
    g.DrawRectangle(linePen, linePenWidth / 2, linePenWidth / 2, width - linePenWidth - 1, height - linePenWidth - 1);

    return bmp;
  }

  public static string GetImageUrl(string cardName)
    => ImageEndpoint + Uri.EscapeDataString(cardName) + ImageParams;
}
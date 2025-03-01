using Newtonsoft.Json;
using System.Diagnostics;

namespace MoxMatrix
{
  public class CardResponse
  {
    [JsonProperty("image_uris")] public ImageUris ImageUris { get; set; }
  }

  public class ImageUris
  {
    [JsonProperty("normal")] public string Normal { get; set; }
  }

  public partial class ImageForm : Form
  {
    private HttpClient httpClient;

    private Dictionary<string, Image> imageDictionary;
    private int maxCachedImages = 100;

    private const string ImageEndpoint = "https://api.scryfall.com/cards/named?exact=";
    private const string ImageParams = "&format=image";

    public ImageForm()
    {
      InitializeComponent();

      httpClient = new HttpClient();
      System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

      imageDictionary = new Dictionary<string, Image>();
    }

    public bool SetPicture(string cardName)
    {
      pictureBox1.Image = null;

      if (imageDictionary.ContainsKey(cardName))
      {
        pictureBox1.Image = imageDictionary[cardName];
        Tag = cardName;
        return true;
      }

      try
      {
        var image = DownloadImage(cardName);

        if (imageDictionary.Count >= maxCachedImages)
        {
          imageDictionary.Clear();
        }

        pictureBox1.Image = image;
        Tag = cardName;
        return true;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }

      return false;
    }

    private void ImageForm_Leave(object sender, EventArgs e)
    {
      Visible = false;
    }

    private void ImageForm_MouseDown(object sender, MouseEventArgs e)
    {
      Visible = false;
    }

    private void ImageForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
      {
        Visible = false;
      }
    }

    private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button is MouseButtons.Right or MouseButtons.Middle)
      {
        var url = ImageEndpoint + Uri.EscapeDataString((string) Tag) + ImageParams;

        var args = "/C start " + url.Replace("&", "^&");

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

      Visible = false;
    }

    public bool DoesImageCacheContain(string cardName)
      => imageDictionary.ContainsKey(cardName);

    public bool AddImageToCache(string cardName, Image image)
    {
      if (imageDictionary.Count >= maxCachedImages)
      {
        return false;
      }

      imageDictionary[cardName] = image;
      return true;
    }

    public Image DownloadImage(string cardName)
    {
      var imageUrl = ImageEndpoint + Uri.EscapeDataString(cardName) + ImageParams;
      var request = new HttpRequestMessage(HttpMethod.Get, imageUrl);
      request.Headers.Add("User-Agent", "YourAppName/1.0 (your-email@example.com)");
      request.Headers.Add("Accept", "application/json;q=0.9,*/*;q=0.8");
      var response = httpClient.SendAsync(request).GetAwaiter().GetResult();
      using var imageStream = response.Content.ReadAsStream();
      var image = Image.FromStream(imageStream);
      return image;
    }
  }
}

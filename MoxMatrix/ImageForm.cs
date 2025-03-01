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
    private readonly ImageCache _imageCache;

    public ImageForm()
    {
      InitializeComponent();

      _imageCache = new ImageCache();
    }

    public void SetPicture(string cardName)
    {
      pictureBox1.Image = _imageCache.Get(cardName);
      Tag = cardName;
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
        var url = ImageCache.GetImageUrl((string)Tag);

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

    public async void UpdateImageCache(List<string> cardNames)
    {
      await Task.Run(() => _imageCache.StartCachingThread(cardNames));
    }
  }
}

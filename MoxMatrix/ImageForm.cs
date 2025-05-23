﻿using Newtonsoft.Json;
using System.Diagnostics;

namespace MoxMatrix
{
  public class CardResponse
  {
    [JsonProperty("image_uris")] public required ImageUris ImageUris { get; set; }
  }

  public class ImageUris
  {
    [JsonProperty("normal")] public required string Normal { get; set; }
  }

  public partial class ImageForm : Form
  {
    private readonly ImageCache _imageCache = new();

    public bool IsMouseDownLeft;
    private Point _mouseDownLocation;
    public Point? LastImagePosition;

    public ImageForm()
    {
      InitializeComponent();
    }

    public void SetPicture(string cardName)
    {
      Tag = cardName;

      pictureBox1.Image = _imageCache.Get(cardName);
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
      if (e.Button == MouseButtons.Left)
      {
        _mouseDownLocation = e.Location;
        IsMouseDownLeft = true;
      }
      else if (e.Button == MouseButtons.Middle)
      {
        LaunchImageInBrowser();
      }
      else if (e.Button == MouseButtons.Right)
      {
        Visible = false;
      }
    }

    private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
    {
      if (IsMouseDownLeft)
      {
        var newX = Cursor.Position.X - _mouseDownLocation.X;
        var newY = Cursor.Position.Y - _mouseDownLocation.Y;
        LastImagePosition = Location = new Point(newX, newY);
      }
    }

    private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button is MouseButtons.Left)
      {
        IsMouseDownLeft = false;
      }
    }

    private void LaunchImageInBrowser()
    {
      var url = ImageCache.GetImageUrl((string)Tag!);
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

    public async void UpdateImageCache(List<string> cardNames)
    {
      await Task.Run(() => _imageCache.StartCachingThread(cardNames));
    }
  }
}

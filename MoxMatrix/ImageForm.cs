using System.ComponentModel;

namespace MoxMatrix
{
  public partial class ImageForm : Form
  {
    public ImageForm()
    {
      InitializeComponent();
    }

    public void SetPicture(string imageUrl)
    {
      pictureBox1.Load(imageUrl);
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
      Visible = false;
    }
  }
}

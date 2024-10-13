namespace MoxMatrix
{
  public partial class ImageForm : Form
  {
    public ImageForm()
    {
      InitializeComponent();
    }

    public bool SetPicture(string imageUrl)
    {
      //AL.
      //TODO - Fix break on "https://c1.scryfall.com/file/scryfall-cards/large/front/5/e/5e6fac09-34e1-495a-ba70-8110845fbefb.jpg" 

      //var qindex = imageUrl.IndexOf('?');
      //if (qindex != -1)
      //{
      //  imageUrl = imageUrl.Substring(0, qindex);
      //}

      try
      {
        pictureBox1.Load(imageUrl);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }

      return true;
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

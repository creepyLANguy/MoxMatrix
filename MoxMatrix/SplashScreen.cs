using MoxMatrix.Properties;

namespace MoxMatrix
{
  public sealed partial class SplashScreen : Form
  {
    public SplashScreen()
    {
      InitializeComponent();

      Visible = false;

      pictureBox1.Image = Resources.splash;
      pictureBox1.BackColor = Color.Transparent;

      BackColor = TransparencyKey = Color.LimeGreen;

      Icon = Resources.icon;
      FormBorderStyle = FormBorderStyle.None;
      Size = pictureBox1.Image.Size;
      TopLevel = true;
      TopMost = true;
      ShowInTaskbar = false;

      CenterToScreen();

      Visible = true;
    }
  }
}

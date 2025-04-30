using MoxMatrix.Properties;
using MoxMatrix.Upgrade;

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

      progressBar.Visible = false;

      lbl_version.Text = @"v" + UpgradeUtils.GetSemanticVersionFromCurrentExecutable();

      Visible = true;
    }

    public void SetProgress(int value)
    {
      Refresh();

      progressBar.Visible = value > 0;

      progressBar.Value = value > 0 && value <= progressBar.Maximum ? value : progressBar.Value;

      Refresh();
    }
  }
}

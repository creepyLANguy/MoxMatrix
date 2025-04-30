public class TransparentOverlay : UserControl
{
  private Label loadingLabel;

  public TransparentOverlay()
  {
    SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
    BackColor = Color.FromArgb(128, 0, 0, 50);
    Dock = DockStyle.Fill;
    Visible = false;

    loadingLabel = new Label
    {
      Text = "Processing...",
      ForeColor = Color.White,
      Font = new Font("Segoe UI", 28, FontStyle.Bold),
      BackColor = Color.Transparent,
      AutoSize = true
    };
    Controls.Add(loadingLabel);

    Resize += (s, e) => CenterLabel();
  }

  private void CenterLabel()
  {
    loadingLabel.Location = new Point(
      (Width - loadingLabel.Width) / 2,
      (Height - loadingLabel.Height) / 2
    );
  }

  protected override CreateParams CreateParams
  {
    get
    {
      var cp = base.CreateParams;
      cp.ExStyle |= 0x20; // WS_EX_TRANSPARENT
      return cp;
    }
  }

  protected override void OnPaintBackground(PaintEventArgs e)
  {
    using var brush = new SolidBrush(BackColor);
    e.Graphics.FillRectangle(brush, this.ClientRectangle);
  }
}
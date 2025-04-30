namespace MoxMatrix
{
  sealed partial class SplashScreen
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      pictureBox1 = new PictureBox();
      progressBar = new ProgressBar();
      lbl_version = new Label();
      ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
      SuspendLayout();
      // 
      // pictureBox1
      // 
      pictureBox1.Dock = DockStyle.Fill;
      pictureBox1.Location = new Point(0, 0);
      pictureBox1.Name = "pictureBox1";
      pictureBox1.Size = new Size(800, 450);
      pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
      pictureBox1.TabIndex = 0;
      pictureBox1.TabStop = false;
      // 
      // progressBar
      // 
      progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      progressBar.Location = new Point(0, 435);
      progressBar.Name = "progressBar";
      progressBar.Size = new Size(800, 15);
      progressBar.Step = 1;
      progressBar.Style = ProgressBarStyle.Continuous;
      progressBar.TabIndex = 1;
      // 
      // lbl_version
      // 
      lbl_version.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      lbl_version.AutoSize = true;
      lbl_version.BackColor = Color.FromArgb(6, 14, 37);
      lbl_version.Font = new Font("Consolas", 10F);
      lbl_version.ForeColor = Color.White;
      lbl_version.Location = new Point(732, 9);
      lbl_version.Name = "lbl_version";
      lbl_version.Size = new Size(56, 17);
      lbl_version.TabIndex = 2;
      lbl_version.Text = "v0.0.0";
      // 
      // SplashScreen
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Controls.Add(lbl_version);
      Controls.Add(progressBar);
      Controls.Add(pictureBox1);
      DoubleBuffered = true;
      Name = "SplashScreen";
      Text = "SplashScreen";
      ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private PictureBox pictureBox1;
    private ProgressBar progressBar;
    private Label lbl_version;
  }
}
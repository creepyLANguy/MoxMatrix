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
      progressBar.Location = new Point(0, 427);
      progressBar.Name = "progressBar";
      progressBar.Size = new Size(800, 23);
      progressBar.Step = 1;
      progressBar.Style = ProgressBarStyle.Continuous;
      progressBar.TabIndex = 1;
      // 
      // SplashScreen
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Controls.Add(progressBar);
      Controls.Add(pictureBox1);
      DoubleBuffered = true;
      Name = "SplashScreen";
      Text = "SplashScreen";
      ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private PictureBox pictureBox1;
    private ProgressBar progressBar;
  }
}
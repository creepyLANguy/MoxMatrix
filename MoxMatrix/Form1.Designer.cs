using System.Resources;
using MoxMatrix.Properties;

namespace MoxMatrix
{
  partial class Form1
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      inputBox = new TextBox();
      btn_go = new Button();
      SuspendLayout();
      // 
      // inputBox
      // 
      inputBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      inputBox.Location = new Point(12, 12);
      inputBox.Multiline = true;
      inputBox.Name = "inputBox";
      inputBox.PlaceholderText = "Input each card name on a new line...";
      inputBox.ScrollBars = ScrollBars.Both;
      inputBox.Size = new Size(422, 263);
      inputBox.TabIndex = 0;
      // 
      // btn_go
      // 
      btn_go.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      btn_go.Location = new Point(12, 281);
      btn_go.Name = "btn_go";
      btn_go.Size = new Size(422, 55);
      btn_go.TabIndex = 1;
      btn_go.Text = "Go";
      btn_go.UseVisualStyleBackColor = true;
      btn_go.Click += btn_go_Click;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(446, 871);
      Controls.Add(btn_go);
      Controls.Add(inputBox);
      Name = "Form1";
      Text = "Mox Matrix (beta)";
      Load += Form1_Load;
      ResumeLayout(false);
      PerformLayout();
      Icon = Resources.icon;
    }

    #endregion

    private TextBox inputBox;
    private Button btn_go;
  }
}
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
      DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
      inputBox = new TextBox();
      btn_go = new Button();
      dataGridView1 = new DataGridView();
      btn_save = new Button();
      textBox1 = new TextBox();
      textBox2 = new TextBox();
      label1 = new Label();
      label2 = new Label();
      label3 = new Label();
      ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
      SuspendLayout();
      // 
      // inputBox
      // 
      inputBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
      inputBox.Location = new Point(12, 27);
      inputBox.Multiline = true;
      inputBox.Name = "inputBox";
      inputBox.PlaceholderText = "Input each card name on a new line...";
      inputBox.ScrollBars = ScrollBars.Both;
      inputBox.Size = new Size(526, 248);
      inputBox.TabIndex = 0;
      // 
      // btn_go
      // 
      btn_go.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      btn_go.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
      btn_go.Location = new Point(12, 281);
      btn_go.Name = "btn_go";
      btn_go.Size = new Size(1557, 55);
      btn_go.TabIndex = 1;
      btn_go.Text = "Query Prices";
      btn_go.UseVisualStyleBackColor = true;
      btn_go.Click += btn_go_Click;
      // 
      // dataGridView1
      // 
      dataGridView1.AllowUserToOrderColumns = true;
      dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      dataGridView1.BackgroundColor = SystemColors.Control;
      dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = SystemColors.Window;
      dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
      dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
      dataGridViewCellStyle1.Format = "C2";
      dataGridViewCellStyle1.NullValue = null;
      dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
      dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
      dataGridView1.Location = new Point(12, 342);
      dataGridView1.Name = "dataGridView1";
      dataGridView1.RowTemplate.Height = 25;
      dataGridView1.Size = new Size(1557, 456);
      dataGridView1.TabIndex = 2;
      dataGridView1.RowPrePaint += dataGridView1_RowPrePaint;
      // 
      // btn_save
      // 
      btn_save.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      btn_save.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
      btn_save.Location = new Point(12, 804);
      btn_save.Name = "btn_save";
      btn_save.Size = new Size(1557, 55);
      btn_save.TabIndex = 3;
      btn_save.Text = "Export CSV ";
      btn_save.UseVisualStyleBackColor = true;
      btn_save.Click += btn_save_Click;
      // 
      // textBox1
      // 
      textBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
      textBox1.Location = new Point(544, 27);
      textBox1.Multiline = true;
      textBox1.Name = "textBox1";
      textBox1.PlaceholderText = "Out of stock or no matches found...";
      textBox1.ReadOnly = true;
      textBox1.ScrollBars = ScrollBars.Both;
      textBox1.Size = new Size(523, 248);
      textBox1.TabIndex = 4;
      // 
      // textBox2
      // 
      textBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      textBox2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
      textBox2.Location = new Point(1073, 27);
      textBox2.Multiline = true;
      textBox2.Name = "textBox2";
      textBox2.PlaceholderText = "Stores Summary...";
      textBox2.ReadOnly = true;
      textBox2.ScrollBars = ScrollBars.Both;
      textBox2.Size = new Size(496, 248);
      textBox2.TabIndex = 5;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(12, 9);
      label1.Name = "label1";
      label1.Size = new Size(198, 15);
      label1.TabIndex = 6;
      label1.Text = "Input each card name on a new line:";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(544, 9);
      label2.Name = "label2";
      label2.Size = new Size(189, 15);
      label2.TabIndex = 7;
      label2.Text = "Out of stock or no matches found:";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(1073, 9);
      label3.Name = "label3";
      label3.Size = new Size(96, 15);
      label3.TabIndex = 8;
      label3.Text = "Stores Summary:";
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(1581, 871);
      Controls.Add(label3);
      Controls.Add(label2);
      Controls.Add(label1);
      Controls.Add(textBox2);
      Controls.Add(textBox1);
      Controls.Add(btn_save);
      Controls.Add(dataGridView1);
      Controls.Add(btn_go);
      Controls.Add(inputBox);
      Icon = Resources.icon;
      Name = "Form1";
      Text = "Mox Matrix (beta)";
      Load += Form1_Load;
      ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private TextBox inputBox;
    private Button btn_go;
    private DataGridView dataGridView1;
    private Button btn_save;
    private TextBox textBox1;
    private TextBox textBox2;
    private Label label1;
    private Label label2;
    private Label label3;
  }
}
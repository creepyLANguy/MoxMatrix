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
      dataGridView1 = new DataGridView();
      btn_exportCSV = new Button();
      txt_urls = new RichTextBox();
      btn_saveUrls = new Button();
      label5 = new Label();
      btn_foils = new CheckBox();
      label1 = new Label();
      txt_outOfStock = new TextBox();
      txt_unknownCards = new TextBox();
      txt_storesSummaries = new TextBox();
      inputBox = new TextBox();
      label2 = new Label();
      label4 = new Label();
      label3 = new Label();
      btn_go = new Button();
      splitContainer1 = new SplitContainer();
      ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
      ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
      splitContainer1.Panel1.SuspendLayout();
      splitContainer1.Panel2.SuspendLayout();
      splitContainer1.SuspendLayout();
      SuspendLayout();
      // 
      // dataGridView1
      // 
      dataGridView1.AllowUserToAddRows = false;
      dataGridView1.AllowUserToDeleteRows = false;
      dataGridView1.AllowUserToOrderColumns = true;
      dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
      dataGridView1.BackgroundColor = SystemColors.Control;
      dataGridView1.BorderStyle = BorderStyle.None;
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
      dataGridView1.Location = new Point(12, 14);
      dataGridView1.Name = "dataGridView1";
      dataGridView1.RowTemplate.Height = 25;
      dataGridView1.Size = new Size(1259, 336);
      dataGridView1.TabIndex = 2;
      dataGridView1.CellClick += dataGridView1_CellClick;
      dataGridView1.CellEnter += dataGridView1_CellEnter;
      dataGridView1.RowPrePaint += dataGridView1_RowPrePaint;
      // 
      // btn_exportCSV
      // 
      btn_exportCSV.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      btn_exportCSV.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
      btn_exportCSV.Location = new Point(12, 356);
      btn_exportCSV.Name = "btn_exportCSV";
      btn_exportCSV.Size = new Size(1259, 55);
      btn_exportCSV.TabIndex = 3;
      btn_exportCSV.Text = "Export CSV ";
      btn_exportCSV.UseVisualStyleBackColor = true;
      btn_exportCSV.Click += btn_exportCSV_Click;
      // 
      // txt_urls
      // 
      txt_urls.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      txt_urls.BorderStyle = BorderStyle.FixedSingle;
      txt_urls.Location = new Point(1289, 27);
      txt_urls.Name = "txt_urls";
      txt_urls.ReadOnly = true;
      txt_urls.Size = new Size(311, 880);
      txt_urls.TabIndex = 12;
      txt_urls.Text = "";
      txt_urls.LinkClicked += txt_urls_LinkClicked;
      // 
      // btn_saveUrls
      // 
      btn_saveUrls.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      btn_saveUrls.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
      btn_saveUrls.Location = new Point(1289, 913);
      btn_saveUrls.Name = "btn_saveUrls";
      btn_saveUrls.Size = new Size(311, 55);
      btn_saveUrls.TabIndex = 11;
      btn_saveUrls.Text = "Save URLs";
      btn_saveUrls.UseVisualStyleBackColor = true;
      btn_saveUrls.Click += btn_saveUrls_Click;
      // 
      // label5
      // 
      label5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      label5.AutoSize = true;
      label5.Location = new Point(1289, 9);
      label5.Name = "label5";
      label5.Size = new Size(33, 15);
      label5.TabIndex = 10;
      label5.Text = "URLs";
      // 
      // btn_foils
      // 
      btn_foils.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      btn_foils.AutoSize = true;
      btn_foils.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
      btn_foils.Location = new Point(284, 490);
      btn_foils.Name = "btn_foils";
      btn_foils.Size = new Size(120, 25);
      btn_foils.TabIndex = 0;
      btn_foils.Text = "OnlyFoils ✨";
      btn_foils.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(12, 9);
      label1.Name = "label1";
      label1.Size = new Size(292, 15);
      label1.TabIndex = 6;
      label1.Text = "Input each card name (or partial name) on a new line: ";
      // 
      // txt_outOfStock
      // 
      txt_outOfStock.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      txt_outOfStock.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
      txt_outOfStock.Location = new Point(699, 27);
      txt_outOfStock.Multiline = true;
      txt_outOfStock.Name = "txt_outOfStock";
      txt_outOfStock.ReadOnly = true;
      txt_outOfStock.ScrollBars = ScrollBars.Both;
      txt_outOfStock.Size = new Size(283, 502);
      txt_outOfStock.TabIndex = 5;
      // 
      // txt_unknownCards
      // 
      txt_unknownCards.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      txt_unknownCards.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
      txt_unknownCards.Location = new Point(410, 27);
      txt_unknownCards.Multiline = true;
      txt_unknownCards.Name = "txt_unknownCards";
      txt_unknownCards.ReadOnly = true;
      txt_unknownCards.ScrollBars = ScrollBars.Both;
      txt_unknownCards.Size = new Size(283, 502);
      txt_unknownCards.TabIndex = 4;
      // 
      // txt_storesSummaries
      // 
      txt_storesSummaries.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      txt_storesSummaries.Location = new Point(988, 27);
      txt_storesSummaries.Multiline = true;
      txt_storesSummaries.Name = "txt_storesSummaries";
      txt_storesSummaries.ReadOnly = true;
      txt_storesSummaries.ScrollBars = ScrollBars.Both;
      txt_storesSummaries.Size = new Size(283, 502);
      txt_storesSummaries.TabIndex = 6;
      // 
      // inputBox
      // 
      inputBox.AllowDrop = true;
      inputBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      inputBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
      inputBox.Location = new Point(12, 27);
      inputBox.Multiline = true;
      inputBox.Name = "inputBox";
      inputBox.ScrollBars = ScrollBars.Both;
      inputBox.Size = new Size(392, 441);
      inputBox.TabIndex = 0;
      inputBox.DragDrop += inputBox_DragDrop;
      inputBox.DragOver += inputBox_DragOver;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(410, 9);
      label2.Name = "label2";
      label2.Size = new Size(202, 15);
      label2.TabIndex = 7;
      label2.Text = "Unknown cards - check your spelling";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(699, 9);
      label4.Name = "label4";
      label4.Size = new Size(72, 15);
      label4.TabIndex = 7;
      label4.Text = "Out of stock";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(988, 9);
      label3.Name = "label3";
      label3.Size = new Size(96, 15);
      label3.TabIndex = 8;
      label3.Text = "Store Summaries";
      // 
      // btn_go
      // 
      btn_go.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      btn_go.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
      btn_go.Location = new Point(12, 474);
      btn_go.Name = "btn_go";
      btn_go.Size = new Size(266, 55);
      btn_go.TabIndex = 1;
      btn_go.Text = "[query]";
      btn_go.UseVisualStyleBackColor = true;
      btn_go.Click += btn_go_Click;
      // 
      // splitContainer1
      // 
      splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      splitContainer1.Location = new Point(0, 0);
      splitContainer1.Name = "splitContainer1";
      splitContainer1.Orientation = Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      splitContainer1.Panel1.Controls.Add(btn_go);
      splitContainer1.Panel1.Controls.Add(label3);
      splitContainer1.Panel1.Controls.Add(label4);
      splitContainer1.Panel1.Controls.Add(label2);
      splitContainer1.Panel1.Controls.Add(inputBox);
      splitContainer1.Panel1.Controls.Add(txt_storesSummaries);
      splitContainer1.Panel1.Controls.Add(txt_unknownCards);
      splitContainer1.Panel1.Controls.Add(txt_outOfStock);
      splitContainer1.Panel1.Controls.Add(label1);
      splitContainer1.Panel1.Controls.Add(btn_foils);
      // 
      // splitContainer1.Panel2
      // 
      splitContainer1.Panel2.Controls.Add(btn_exportCSV);
      splitContainer1.Panel2.Controls.Add(dataGridView1);
      splitContainer1.Size = new Size(1283, 971);
      splitContainer1.SplitterDistance = 547;
      splitContainer1.SplitterWidth = 10;
      splitContainer1.TabIndex = 9;
      splitContainer1.Paint += splitContainer1_Paint;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(1612, 983);
      Controls.Add(splitContainer1);
      Controls.Add(txt_urls);
      Controls.Add(btn_saveUrls);
      Controls.Add(label5);
      Icon = Resources.icon;
      Name = "Form1";
      StartPosition = FormStartPosition.CenterScreen;
      Text = "Mox Matrix (beta)";
      Load += Form1_Load;
      ResizeBegin += Form1_ResizeBegin;
      ResizeEnd += Form1_ResizeEnd;
      ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
      splitContainer1.Panel1.ResumeLayout(false);
      splitContainer1.Panel1.PerformLayout();
      splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
      splitContainer1.ResumeLayout(false);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private DataGridView dataGridView1;
    private Button btn_exportCSV;
    private RichTextBox txt_urls;
    private Button btn_saveUrls;
    private Label label5;
    private CheckBox btn_foils;
    private Label label1;
    private TextBox txt_outOfStock;
    private TextBox txt_unknownCards;
    private TextBox txt_storesSummaries;
    private TextBox inputBox;
    private Label label2;
    private Label label4;
    private Label label3;
    private Button btn_go;
    private SplitContainer splitContainer1;
  }
}
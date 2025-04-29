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
      panel1 = new Panel();
      panel2 = new Panel();
      cb_individualsAll = new CheckBox();
      cb_businessesAll = new CheckBox();
      label6 = new Label();
      txt_errorFetching = new TextBox();
      cl_individuals = new CheckedListBox();
      cl_businesses = new CheckedListBox();
      label8 = new Label();
      label7 = new Label();
      cb_OracleVersions = new ComboBox();
      lbl_OracleVersion = new Label();
      txt_TopStoresToConsider = new MaskedTextBox();
      lbl_TopStoresToConsider = new Label();
      btn_exportBuyList = new Button();
      ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
      ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
      splitContainer1.Panel1.SuspendLayout();
      splitContainer1.Panel2.SuspendLayout();
      splitContainer1.SuspendLayout();
      panel2.SuspendLayout();
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
      dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
      dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
      dataGridViewCellStyle1.Format = "C2";
      dataGridViewCellStyle1.NullValue = null;
      dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
      dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
      dataGridView1.Location = new Point(12, 14);
      dataGridView1.Name = "dataGridView1";
      dataGridView1.Size = new Size(1259, 340);
      dataGridView1.TabIndex = 2;
      dataGridView1.CellClick += dataGridView1_CellClick;
      dataGridView1.CellEnter += dataGridView1_CellEnter;
      dataGridView1.ColumnHeaderMouseClick += dataGridView1_ColumnHeaderMouseClick;
      dataGridView1.RowPrePaint += dataGridView1_RowPrePaint;
      dataGridView1.KeyUp += dataGridView1_KeyUp;
      // 
      // btn_exportCSV
      // 
      btn_exportCSV.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      btn_exportCSV.Font = new Font("Segoe UI", 12F);
      btn_exportCSV.Location = new Point(12, 360);
      btn_exportCSV.Name = "btn_exportCSV";
      btn_exportCSV.Size = new Size(487, 55);
      btn_exportCSV.TabIndex = 3;
      btn_exportCSV.Text = "Export CSV";
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
      txt_urls.Size = new Size(345, 890);
      txt_urls.TabIndex = 12;
      txt_urls.Text = "";
      txt_urls.LinkClicked += txt_urls_LinkClicked;
      // 
      // btn_saveUrls
      // 
      btn_saveUrls.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      btn_saveUrls.Font = new Font("Segoe UI", 12F);
      btn_saveUrls.Location = new Point(1289, 923);
      btn_saveUrls.Name = "btn_saveUrls";
      btn_saveUrls.Size = new Size(345, 55);
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
      btn_foils.Font = new Font("Segoe UI", 12F);
      btn_foils.Location = new Point(84, 21);
      btn_foils.Name = "btn_foils";
      btn_foils.Size = new Size(120, 25);
      btn_foils.TabIndex = 2;
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
      txt_outOfStock.Font = new Font("Segoe UI", 12F);
      txt_outOfStock.Location = new Point(699, 27);
      txt_outOfStock.Multiline = true;
      txt_outOfStock.Name = "txt_outOfStock";
      txt_outOfStock.ReadOnly = true;
      txt_outOfStock.ScrollBars = ScrollBars.Both;
      txt_outOfStock.Size = new Size(283, 172);
      txt_outOfStock.TabIndex = 5;
      // 
      // txt_unknownCards
      // 
      txt_unknownCards.Font = new Font("Segoe UI", 12F);
      txt_unknownCards.ForeColor = SystemColors.WindowText;
      txt_unknownCards.Location = new Point(699, 220);
      txt_unknownCards.Multiline = true;
      txt_unknownCards.Name = "txt_unknownCards";
      txt_unknownCards.ReadOnly = true;
      txt_unknownCards.ScrollBars = ScrollBars.Both;
      txt_unknownCards.Size = new Size(283, 189);
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
      txt_storesSummaries.Size = new Size(283, 508);
      txt_storesSummaries.TabIndex = 6;
      // 
      // inputBox
      // 
      inputBox.AllowDrop = true;
      inputBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      inputBox.Font = new Font("Segoe UI", 12F);
      inputBox.Location = new Point(12, 27);
      inputBox.Multiline = true;
      inputBox.Name = "inputBox";
      inputBox.ScrollBars = ScrollBars.Both;
      inputBox.Size = new Size(392, 447);
      inputBox.TabIndex = 0;
      inputBox.DragDrop += inputBox_DragDrop;
      inputBox.DragOver += inputBox_DragOver;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(699, 202);
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
      btn_go.Font = new Font("Segoe UI", 12F);
      btn_go.Location = new Point(12, 480);
      btn_go.Name = "btn_go";
      btn_go.Size = new Size(392, 55);
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
      splitContainer1.Panel1.Controls.Add(panel1);
      splitContainer1.Panel1.Controls.Add(panel2);
      splitContainer1.Panel1.Controls.Add(cb_individualsAll);
      splitContainer1.Panel1.Controls.Add(cb_businessesAll);
      splitContainer1.Panel1.Controls.Add(label6);
      splitContainer1.Panel1.Controls.Add(txt_errorFetching);
      splitContainer1.Panel1.Controls.Add(btn_go);
      splitContainer1.Panel1.Controls.Add(label3);
      splitContainer1.Panel1.Controls.Add(label4);
      splitContainer1.Panel1.Controls.Add(label2);
      splitContainer1.Panel1.Controls.Add(inputBox);
      splitContainer1.Panel1.Controls.Add(txt_storesSummaries);
      splitContainer1.Panel1.Controls.Add(txt_unknownCards);
      splitContainer1.Panel1.Controls.Add(txt_outOfStock);
      splitContainer1.Panel1.Controls.Add(label1);
      splitContainer1.Panel1.Controls.Add(cl_individuals);
      splitContainer1.Panel1.Controls.Add(cl_businesses);
      splitContainer1.Panel1.Controls.Add(label8);
      splitContainer1.Panel1.Controls.Add(label7);
      // 
      // splitContainer1.Panel2
      // 
      splitContainer1.Panel2.AutoScroll = true;
      splitContainer1.Panel2.Controls.Add(cb_OracleVersions);
      splitContainer1.Panel2.Controls.Add(lbl_OracleVersion);
      splitContainer1.Panel2.Controls.Add(txt_TopStoresToConsider);
      splitContainer1.Panel2.Controls.Add(lbl_TopStoresToConsider);
      splitContainer1.Panel2.Controls.Add(btn_exportBuyList);
      splitContainer1.Panel2.Controls.Add(btn_exportCSV);
      splitContainer1.Panel2.Controls.Add(dataGridView1);
      splitContainer1.Size = new Size(1283, 983);
      splitContainer1.SplitterDistance = 553;
      splitContainer1.SplitterWidth = 10;
      splitContainer1.TabIndex = 9;
      splitContainer1.Paint += splitContainer1_Paint;
      // 
      // panel1
      // 
      panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      panel1.BackColor = SystemColors.ControlLight;
      panel1.Location = new Point(12, 541);
      panel1.Name = "panel1";
      panel1.Size = new Size(1259, 2);
      panel1.TabIndex = 9;
      // 
      // panel2
      // 
      panel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      panel2.Controls.Add(btn_foils);
      panel2.Location = new Point(403, 480);
      panel2.Name = "panel2";
      panel2.Size = new Size(295, 60);
      panel2.TabIndex = 20;
      // 
      // cb_individualsAll
      // 
      cb_individualsAll.AutoSize = true;
      cb_individualsAll.Checked = true;
      cb_individualsAll.CheckState = CheckState.Checked;
      cb_individualsAll.Location = new Point(482, 343);
      cb_individualsAll.Name = "cb_individualsAll";
      cb_individualsAll.Size = new Size(40, 19);
      cb_individualsAll.TabIndex = 19;
      cb_individualsAll.Text = "All";
      cb_individualsAll.UseVisualStyleBackColor = true;
      cb_individualsAll.CheckedChanged += cb_individualsAll_CheckedChanged;
      // 
      // cb_businessesAll
      // 
      cb_businessesAll.AutoSize = true;
      cb_businessesAll.Checked = true;
      cb_businessesAll.CheckState = CheckState.Checked;
      cb_businessesAll.Location = new Point(482, 8);
      cb_businessesAll.Name = "cb_businessesAll";
      cb_businessesAll.Size = new Size(40, 19);
      cb_businessesAll.TabIndex = 18;
      cb_businessesAll.Text = "All";
      cb_businessesAll.UseVisualStyleBackColor = true;
      cb_businessesAll.CheckedChanged += cb_businessesAll_CheckedChanged;
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Location = new Point(699, 412);
      label6.Name = "label6";
      label6.Size = new Size(278, 15);
      label6.TabIndex = 11;
      label6.Text = "NETWORK ERROR fetching these cards- please retry";
      // 
      // txt_errorFetching
      // 
      txt_errorFetching.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      txt_errorFetching.Font = new Font("Segoe UI", 12F);
      txt_errorFetching.ForeColor = SystemColors.WindowText;
      txt_errorFetching.Location = new Point(699, 430);
      txt_errorFetching.Multiline = true;
      txt_errorFetching.Name = "txt_errorFetching";
      txt_errorFetching.ReadOnly = true;
      txt_errorFetching.ScrollBars = ScrollBars.Both;
      txt_errorFetching.Size = new Size(283, 105);
      txt_errorFetching.TabIndex = 10;
      // 
      // cl_individuals
      // 
      cl_individuals.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      cl_individuals.CheckOnClick = true;
      cl_individuals.FormattingEnabled = true;
      cl_individuals.HorizontalScrollbar = true;
      cl_individuals.Location = new Point(410, 362);
      cl_individuals.Name = "cl_individuals";
      cl_individuals.Size = new Size(283, 112);
      cl_individuals.TabIndex = 17;
      cl_individuals.UseTabStops = false;
      cl_individuals.KeyUp += cl_individuals_KeyUp;
      cl_individuals.MouseUp += cl_individuals_MouseUp;
      // 
      // cl_businesses
      // 
      cl_businesses.CheckOnClick = true;
      cl_businesses.FormattingEnabled = true;
      cl_businesses.HorizontalScrollbar = true;
      cl_businesses.Location = new Point(410, 27);
      cl_businesses.Name = "cl_businesses";
      cl_businesses.Size = new Size(283, 310);
      cl_businesses.TabIndex = 16;
      cl_businesses.UseTabStops = false;
      cl_businesses.KeyUp += cl_businesses_KeyUp;
      cl_businesses.MouseUp += cl_businesses_MouseUp;
      // 
      // label8
      // 
      label8.AutoSize = true;
      label8.Location = new Point(410, 344);
      label8.Name = "label8";
      label8.Size = new Size(67, 15);
      label8.TabIndex = 13;
      label8.Text = "Individuals:";
      // 
      // label7
      // 
      label7.AutoSize = true;
      label7.Location = new Point(410, 9);
      label7.Name = "label7";
      label7.Size = new Size(66, 15);
      label7.TabIndex = 12;
      label7.Text = "Businesses:";
      // 
      // cb_OracleVersions
      // 
      cb_OracleVersions.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      cb_OracleVersions.Font = new Font("Segoe UI", 18F);
      cb_OracleVersions.FormattingEnabled = true;
      cb_OracleVersions.Location = new Point(1003, 375);
      cb_OracleVersions.Name = "cb_OracleVersions";
      cb_OracleVersions.Size = new Size(121, 40);
      cb_OracleVersions.TabIndex = 15;
      cb_OracleVersions.DropDownClosed += cb_OracleVersions_DropDownClosed;
      // 
      // lbl_OracleVersion
      // 
      lbl_OracleVersion.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      lbl_OracleVersion.AutoSize = true;
      lbl_OracleVersion.Location = new Point(1003, 357);
      lbl_OracleVersion.Name = "lbl_OracleVersion";
      lbl_OracleVersion.Size = new Size(105, 15);
      lbl_OracleVersion.TabIndex = 14;
      lbl_OracleVersion.Text = "Algorithm Version:";
      // 
      // txt_TopStoresToConsider
      // 
      txt_TopStoresToConsider.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      txt_TopStoresToConsider.Font = new Font("Segoe UI", 18F);
      txt_TopStoresToConsider.Location = new Point(1130, 375);
      txt_TopStoresToConsider.Mask = "00";
      txt_TopStoresToConsider.Name = "txt_TopStoresToConsider";
      txt_TopStoresToConsider.Size = new Size(141, 39);
      txt_TopStoresToConsider.TabIndex = 13;
      txt_TopStoresToConsider.TextAlign = HorizontalAlignment.Center;
      txt_TopStoresToConsider.ValidatingType = typeof(int);
      // 
      // lbl_TopStoresToConsider
      // 
      lbl_TopStoresToConsider.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      lbl_TopStoresToConsider.AutoSize = true;
      lbl_TopStoresToConsider.Location = new Point(1127, 357);
      lbl_TopStoresToConsider.Name = "lbl_TopStoresToConsider";
      lbl_TopStoresToConsider.Size = new Size(144, 15);
      lbl_TopStoresToConsider.TabIndex = 6;
      lbl_TopStoresToConsider.Text = "Top 'n' Stores to Consider:";
      // 
      // btn_exportBuyList
      // 
      btn_exportBuyList.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      btn_exportBuyList.Font = new Font("Segoe UI", 12F);
      btn_exportBuyList.Location = new Point(505, 360);
      btn_exportBuyList.Name = "btn_exportBuyList";
      btn_exportBuyList.Size = new Size(492, 55);
      btn_exportBuyList.TabIndex = 4;
      btn_exportBuyList.Text = "Export Optimised Buy List";
      btn_exportBuyList.UseVisualStyleBackColor = true;
      btn_exportBuyList.Click += btn_exportBuyList_Click;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(1646, 983);
      Controls.Add(splitContainer1);
      Controls.Add(txt_urls);
      Controls.Add(btn_saveUrls);
      Controls.Add(label5);
      Icon = Resources.icon;
      Name = "Form1";
      StartPosition = FormStartPosition.CenterScreen;
      Text = "Mox Matrix (beta)";
      Load += Form1_Load;
      Shown += Form1_Shown;
      ResizeEnd += Form1_ResizeEnd;
      Resize += Form1_Resize;
      ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
      splitContainer1.Panel1.ResumeLayout(false);
      splitContainer1.Panel1.PerformLayout();
      splitContainer1.Panel2.ResumeLayout(false);
      splitContainer1.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
      splitContainer1.ResumeLayout(false);
      panel2.ResumeLayout(false);
      panel2.PerformLayout();
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
    private Panel panel1;
    private Label label6;
    private TextBox txt_errorFetching;
    private Label label7;
    private Label label8;
    private CheckedListBox cl_businesses;
    private CheckedListBox cl_individuals;
    private CheckBox cb_individualsAll;
    private CheckBox cb_businessesAll;
    private Panel panel2;
    private Button btn_exportBuyList;
    private Label lbl_TopStoresToConsider;
    private MaskedTextBox txt_TopStoresToConsider;
    private Label lbl_OracleVersion;
    private ComboBox cb_OracleVersions;
  }
}
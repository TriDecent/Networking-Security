namespace CryptographicApp
{
  partial class PlayFairForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayFairForm));
      label1 = new Label();
      label2 = new Label();
      label3 = new Label();
      textBoxPlainText = new TextBox();
      textBoxCipherText = new TextBox();
      textBoxKey = new TextBox();
      buttonGenerateMatrix = new Button();
      buttonEncrypt = new Button();
      buttonDecrypt = new Button();
      tableLayoutPanel5x5 = new TableLayoutPanel();
      label4 = new Label();
      textBoxResult = new TextBox();
      groupBox1 = new GroupBox();
      radioButton6x6 = new RadioButton();
      radioButton5x5 = new RadioButton();
      tableLayoutPanel6x6 = new TableLayoutPanel();
      groupBox1.SuspendLayout();
      SuspendLayout();
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(8, 17);
      label1.Margin = new Padding(2, 0, 2, 0);
      label1.Name = "label1";
      label1.Size = new Size(74, 15);
      label1.TabIndex = 0;
      label1.Text = "PLAIN TEXT: ";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(8, 54);
      label2.Margin = new Padding(2, 0, 2, 0);
      label2.Name = "label2";
      label2.Size = new Size(81, 15);
      label2.TabIndex = 1;
      label2.Text = "CIPHER TEXT: ";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(8, 102);
      label3.Margin = new Padding(2, 0, 2, 0);
      label3.Name = "label3";
      label3.Size = new Size(30, 15);
      label3.TabIndex = 2;
      label3.Text = "KEY:";
      // 
      // textBoxPlainText
      // 
      textBoxPlainText.Location = new Point(104, 12);
      textBoxPlainText.Margin = new Padding(2, 2, 2, 2);
      textBoxPlainText.Multiline = true;
      textBoxPlainText.Name = "textBoxPlainText";
      textBoxPlainText.Size = new Size(382, 29);
      textBoxPlainText.TabIndex = 3;
      // 
      // textBoxCipherText
      // 
      textBoxCipherText.Location = new Point(104, 47);
      textBoxCipherText.Margin = new Padding(2, 2, 2, 2);
      textBoxCipherText.Multiline = true;
      textBoxCipherText.Name = "textBoxCipherText";
      textBoxCipherText.Size = new Size(382, 29);
      textBoxCipherText.TabIndex = 4;
      // 
      // textBoxKey
      // 
      textBoxKey.Location = new Point(104, 100);
      textBoxKey.Margin = new Padding(2, 2, 2, 2);
      textBoxKey.Name = "textBoxKey";
      textBoxKey.Size = new Size(382, 23);
      textBoxKey.TabIndex = 5;
      // 
      // buttonGenerateMatrix
      // 
      buttonGenerateMatrix.Location = new Point(8, 195);
      buttonGenerateMatrix.Margin = new Padding(2, 2, 2, 2);
      buttonGenerateMatrix.Name = "buttonGenerateMatrix";
      buttonGenerateMatrix.Size = new Size(170, 30);
      buttonGenerateMatrix.TabIndex = 6;
      buttonGenerateMatrix.Text = "Generate Matrix Key";
      buttonGenerateMatrix.UseVisualStyleBackColor = true;
      buttonGenerateMatrix.Click += ButtonGenerateMatrix_Click;
      // 
      // buttonEncrypt
      // 
      buttonEncrypt.Location = new Point(201, 195);
      buttonEncrypt.Margin = new Padding(2, 2, 2, 2);
      buttonEncrypt.Name = "buttonEncrypt";
      buttonEncrypt.Size = new Size(92, 30);
      buttonEncrypt.TabIndex = 7;
      buttonEncrypt.Text = "ENCRYPT";
      buttonEncrypt.UseVisualStyleBackColor = true;
      buttonEncrypt.Click += ButtonEncrypt_Click;
      // 
      // buttonDecrypt
      // 
      buttonDecrypt.Location = new Point(321, 195);
      buttonDecrypt.Margin = new Padding(2, 2, 2, 2);
      buttonDecrypt.Name = "buttonDecrypt";
      buttonDecrypt.Size = new Size(92, 30);
      buttonDecrypt.TabIndex = 8;
      buttonDecrypt.Text = "DECRYPT";
      buttonDecrypt.UseVisualStyleBackColor = true;
      buttonDecrypt.Click += ButtonDecrypt_Click;
      // 
      // tableLayoutPanel5x5
      // 
      tableLayoutPanel5x5.ColumnCount = 5;
      tableLayoutPanel5x5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
      tableLayoutPanel5x5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
      tableLayoutPanel5x5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
      tableLayoutPanel5x5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
      tableLayoutPanel5x5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
      tableLayoutPanel5x5.Location = new Point(527, 12);
      tableLayoutPanel5x5.Margin = new Padding(2, 2, 2, 2);
      tableLayoutPanel5x5.Name = "tableLayoutPanel5x5";
      tableLayoutPanel5x5.RowCount = 5;
      tableLayoutPanel5x5.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
      tableLayoutPanel5x5.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
      tableLayoutPanel5x5.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
      tableLayoutPanel5x5.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
      tableLayoutPanel5x5.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
      tableLayoutPanel5x5.Size = new Size(257, 166);
      tableLayoutPanel5x5.TabIndex = 9;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(489, 203);
      label4.Margin = new Padding(2, 0, 2, 0);
      label4.Name = "label4";
      label4.Size = new Size(48, 15);
      label4.TabIndex = 10;
      label4.Text = "RESULT:";
      // 
      // textBoxResult
      // 
      textBoxResult.Location = new Point(489, 228);
      textBoxResult.Margin = new Padding(2, 2, 2, 2);
      textBoxResult.Multiline = true;
      textBoxResult.Name = "textBoxResult";
      textBoxResult.Size = new Size(312, 29);
      textBoxResult.TabIndex = 11;
      // 
      // groupBox1
      // 
      groupBox1.Controls.Add(radioButton6x6);
      groupBox1.Controls.Add(radioButton5x5);
      groupBox1.Location = new Point(8, 133);
      groupBox1.Margin = new Padding(2, 2, 2, 2);
      groupBox1.Name = "groupBox1";
      groupBox1.Padding = new Padding(2, 2, 2, 2);
      groupBox1.Size = new Size(135, 53);
      groupBox1.TabIndex = 12;
      groupBox1.TabStop = false;
      groupBox1.Text = "Choose matrix type";
      // 
      // radioButton6x6
      // 
      radioButton6x6.AutoSize = true;
      radioButton6x6.Location = new Point(78, 23);
      radioButton6x6.Margin = new Padding(2, 2, 2, 2);
      radioButton6x6.Name = "radioButton6x6";
      radioButton6x6.Size = new Size(43, 19);
      radioButton6x6.TabIndex = 1;
      radioButton6x6.TabStop = true;
      radioButton6x6.Text = "6x6";
      radioButton6x6.UseVisualStyleBackColor = true;
      radioButton6x6.CheckedChanged += RadioButton6x6_CheckedChanged;
      // 
      // radioButton5x5
      // 
      radioButton5x5.AutoSize = true;
      radioButton5x5.Location = new Point(4, 23);
      radioButton5x5.Margin = new Padding(2, 2, 2, 2);
      radioButton5x5.Name = "radioButton5x5";
      radioButton5x5.Size = new Size(43, 19);
      radioButton5x5.TabIndex = 0;
      radioButton5x5.TabStop = true;
      radioButton5x5.Text = "5x5";
      radioButton5x5.UseVisualStyleBackColor = true;
      radioButton5x5.CheckedChanged += RadioButton5x5_CheckedChanged;
      // 
      // tableLayoutPanel6x6
      // 
      tableLayoutPanel6x6.ColumnCount = 6;
      tableLayoutPanel6x6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
      tableLayoutPanel6x6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
      tableLayoutPanel6x6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
      tableLayoutPanel6x6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
      tableLayoutPanel6x6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
      tableLayoutPanel6x6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6666679F));
      tableLayoutPanel6x6.Location = new Point(522, 7);
      tableLayoutPanel6x6.Margin = new Padding(2, 2, 2, 2);
      tableLayoutPanel6x6.Name = "tableLayoutPanel6x6";
      tableLayoutPanel6x6.RowCount = 6;
      tableLayoutPanel6x6.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666679F));
      tableLayoutPanel6x6.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666679F));
      tableLayoutPanel6x6.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666679F));
      tableLayoutPanel6x6.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666679F));
      tableLayoutPanel6x6.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666679F));
      tableLayoutPanel6x6.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6666679F));
      tableLayoutPanel6x6.Size = new Size(278, 185);
      tableLayoutPanel6x6.TabIndex = 13;
      // 
      // PlayFairForm
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(812, 273);
      Controls.Add(tableLayoutPanel6x6);
      Controls.Add(groupBox1);
      Controls.Add(textBoxResult);
      Controls.Add(label4);
      Controls.Add(tableLayoutPanel5x5);
      Controls.Add(buttonDecrypt);
      Controls.Add(buttonEncrypt);
      Controls.Add(buttonGenerateMatrix);
      Controls.Add(textBoxKey);
      Controls.Add(textBoxCipherText);
      Controls.Add(textBoxPlainText);
      Controls.Add(label3);
      Controls.Add(label2);
      Controls.Add(label1);
      FormBorderStyle = FormBorderStyle.Fixed3D;
      Icon = (Icon)resources.GetObject("$this.Icon");
      MaximizeBox = false;
      Name = "PlayFairForm";
      Text = "PlayFair";
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Label label1;
    private Label label2;
    private Label label3;
    private TextBox textBoxPlainText;
    private TextBox textBoxCipherText;
    private TextBox textBoxKey;
    private Button buttonGenerateMatrix;
    private Button buttonEncrypt;
    private Button buttonDecrypt;
    private TableLayoutPanel tableLayoutPanel5x5;
    private Label label4;
    private TextBox textBoxResult;
    private GroupBox groupBox1;
    private RadioButton radioButton6x6;
    private RadioButton radioButton5x5;
    private TableLayoutPanel tableLayoutPanel6x6;
  }
}
namespace HashCalculator;

partial class HashCalculatorForm
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
    cbMD5 = new CheckBox();
    cbSHA1 = new CheckBox();
    cbSHA256 = new CheckBox();
    txtMD5Hash = new TextBox();
    txtSHA1Hash = new TextBox();
    txtSHA256Hash = new TextBox();
    lblDataFormat = new Label();
    lblData = new Label();
    txtData = new TextBox();
    cbDataFormat = new ComboBox();
    SuspendLayout();
    // 
    // cbMD5
    // 
    cbMD5.AutoSize = true;
    cbMD5.Font = new Font("Segoe UI", 12F);
    cbMD5.Location = new Point(26, 139);
    cbMD5.Name = "cbMD5";
    cbMD5.Size = new Size(63, 25);
    cbMD5.TabIndex = 0;
    cbMD5.Text = "MD5";
    cbMD5.UseVisualStyleBackColor = true;
    // 
    // cbSHA1
    // 
    cbSHA1.AutoSize = true;
    cbSHA1.Font = new Font("Segoe UI", 12F);
    cbSHA1.Location = new Point(26, 192);
    cbSHA1.Name = "cbSHA1";
    cbSHA1.Size = new Size(74, 25);
    cbSHA1.TabIndex = 1;
    cbSHA1.Text = "SHA-1";
    cbSHA1.UseVisualStyleBackColor = true;
    // 
    // cbSHA256
    // 
    cbSHA256.AutoSize = true;
    cbSHA256.Font = new Font("Segoe UI", 12F);
    cbSHA256.Location = new Point(26, 257);
    cbSHA256.Name = "cbSHA256";
    cbSHA256.Size = new Size(92, 25);
    cbSHA256.TabIndex = 2;
    cbSHA256.Text = "SHA-256";
    cbSHA256.UseVisualStyleBackColor = true;
    // 
    // txtMD5Hash
    // 
    txtMD5Hash.Font = new Font("Segoe UI", 12F);
    txtMD5Hash.Location = new Point(132, 137);
    txtMD5Hash.Name = "txtMD5Hash";
    txtMD5Hash.ReadOnly = true;
    txtMD5Hash.Size = new Size(632, 29);
    txtMD5Hash.TabIndex = 0;
    // 
    // txtSHA1Hash
    // 
    txtSHA1Hash.Font = new Font("Segoe UI", 12F);
    txtSHA1Hash.Location = new Point(132, 192);
    txtSHA1Hash.Name = "txtSHA1Hash";
    txtSHA1Hash.ReadOnly = true;
    txtSHA1Hash.Size = new Size(632, 29);
    txtSHA1Hash.TabIndex = 0;
    // 
    // txtSHA256Hash
    // 
    txtSHA256Hash.Font = new Font("Segoe UI", 12F);
    txtSHA256Hash.Location = new Point(132, 255);
    txtSHA256Hash.Name = "txtSHA256Hash";
    txtSHA256Hash.ReadOnly = true;
    txtSHA256Hash.Size = new Size(632, 29);
    txtSHA256Hash.TabIndex = 3;
    // 
    // lblDataFormat
    // 
    lblDataFormat.AutoSize = true;
    lblDataFormat.Font = new Font("Segoe UI", 12F);
    lblDataFormat.Location = new Point(21, 22);
    lblDataFormat.Name = "lblDataFormat";
    lblDataFormat.Size = new Size(96, 21);
    lblDataFormat.TabIndex = 4;
    lblDataFormat.Text = "Data Format";
    // 
    // lblData
    // 
    lblData.AutoSize = true;
    lblData.Font = new Font("Segoe UI", 12F);
    lblData.Location = new Point(132, 22);
    lblData.Name = "lblData";
    lblData.Size = new Size(42, 21);
    lblData.TabIndex = 5;
    lblData.Text = "Data";
    // 
    // txtData
    // 
    txtData.Font = new Font("Segoe UI", 12F);
    txtData.Location = new Point(132, 60);
    txtData.Name = "txtData";
    txtData.Size = new Size(632, 29);
    txtData.TabIndex = 7;
    // 
    // cbDataFormat
    // 
    cbDataFormat.AutoCompleteMode = AutoCompleteMode.Suggest;
    cbDataFormat.Font = new Font("Segoe UI", 12F);
    cbDataFormat.FormattingEnabled = true;
    cbDataFormat.Items.AddRange(new object[] { "Text", "Hex", "File" });
    cbDataFormat.Location = new Point(21, 60);
    cbDataFormat.Name = "cbDataFormat";
    cbDataFormat.Size = new Size(79, 29);
    cbDataFormat.TabIndex = 8;
    // 
    // HashCalculatorForm
    // 
    AutoScaleDimensions = new SizeF(7F, 15F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(800, 324);
    Controls.Add(cbDataFormat);
    Controls.Add(txtData);
    Controls.Add(lblData);
    Controls.Add(lblDataFormat);
    Controls.Add(txtSHA256Hash);
    Controls.Add(txtSHA1Hash);
    Controls.Add(txtMD5Hash);
    Controls.Add(cbSHA256);
    Controls.Add(cbSHA1);
    Controls.Add(cbMD5);
    FormBorderStyle = FormBorderStyle.Fixed3D;
    MinimizeBox = false;
    Name = "HashCalculatorForm";
    Text = "Hash Code Generator";
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private CheckBox cbMD5;
  private CheckBox cbSHA1;
  private CheckBox cbSHA256;
  private TextBox txtMD5Hash;
  private TextBox txtSHA1Hash;
  private TextBox txtSHA256Hash;
  private Label lblDataFormat;
  private Label lblData;
  private TextBox txtData;
  private ComboBox cbDataFormat;
}

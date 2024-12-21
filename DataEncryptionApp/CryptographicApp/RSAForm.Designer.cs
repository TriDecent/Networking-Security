namespace CryptographicApp
{
  partial class RSAForm
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
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RSAForm));
      cbDataFormat = new ComboBox();
      txtDataOrFilePath = new TextBox();
      lblDataOrFilePath = new Label();
      lblDataFormat = new Label();
      btnImportKey = new Button();
      btnGenerateKey = new Button();
      btnBrowse = new Button();
      progressBar = new ProgressBar();
      lblProgress = new Label();
      lblTimeTook = new Label();
      txtResult = new TextBox();
      lblEncryptedText = new Label();
      btnEncrypt = new Button();
      btnDecrypt = new Button();
      cbPadding = new ComboBox();
      lblPadding = new Label();
      txtImportedKeyName = new TextBox();
      lblKeySize = new Label();
      cbKeySize = new ComboBox();
      timer = new System.Windows.Forms.Timer(components);
      cbUseMultithreading = new CheckBox();
      SuspendLayout();
      // 
      // cbDataFormat
      // 
      cbDataFormat.DropDownStyle = ComboBoxStyle.DropDownList;
      cbDataFormat.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      cbDataFormat.FormattingEnabled = true;
      cbDataFormat.Items.AddRange(new object[] { "Text", "Hex", "File" });
      cbDataFormat.Location = new Point(16, 42);
      cbDataFormat.Name = "cbDataFormat";
      cbDataFormat.Size = new Size(96, 29);
      cbDataFormat.TabIndex = 12;
      // 
      // txtDataOrFilePath
      // 
      txtDataOrFilePath.BorderStyle = BorderStyle.FixedSingle;
      txtDataOrFilePath.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      txtDataOrFilePath.Location = new Point(127, 42);
      txtDataOrFilePath.Name = "txtDataOrFilePath";
      txtDataOrFilePath.Size = new Size(432, 29);
      txtDataOrFilePath.TabIndex = 11;
      // 
      // lblDataOrFilePath
      // 
      lblDataOrFilePath.AutoSize = true;
      lblDataOrFilePath.Font = new Font("Segoe UI", 12F);
      lblDataOrFilePath.Location = new Point(123, 18);
      lblDataOrFilePath.Name = "lblDataOrFilePath";
      lblDataOrFilePath.Size = new Size(42, 21);
      lblDataOrFilePath.TabIndex = 10;
      lblDataOrFilePath.Text = "Data";
      // 
      // lblDataFormat
      // 
      lblDataFormat.AutoSize = true;
      lblDataFormat.Font = new Font("Segoe UI", 12F);
      lblDataFormat.Location = new Point(12, 18);
      lblDataFormat.Name = "lblDataFormat";
      lblDataFormat.Size = new Size(96, 21);
      lblDataFormat.TabIndex = 9;
      lblDataFormat.Text = "Data Format";
      // 
      // btnImportKey
      // 
      btnImportKey.Font = new Font("Segoe UI", 12F);
      btnImportKey.Location = new Point(387, 89);
      btnImportKey.Name = "btnImportKey";
      btnImportKey.Size = new Size(95, 29);
      btnImportKey.TabIndex = 13;
      btnImportKey.Text = "Import Key";
      btnImportKey.UseVisualStyleBackColor = true;
      // 
      // btnGenerateKey
      // 
      btnGenerateKey.Font = new Font("Segoe UI", 12F);
      btnGenerateKey.Location = new Point(268, 89);
      btnGenerateKey.Name = "btnGenerateKey";
      btnGenerateKey.Size = new Size(113, 29);
      btnGenerateKey.TabIndex = 14;
      btnGenerateKey.Text = "Generate Key";
      btnGenerateKey.UseVisualStyleBackColor = true;
      // 
      // btnBrowse
      // 
      btnBrowse.Font = new Font("Segoe UI", 12F);
      btnBrowse.Location = new Point(570, 42);
      btnBrowse.Name = "btnBrowse";
      btnBrowse.Size = new Size(75, 29);
      btnBrowse.TabIndex = 15;
      btnBrowse.Text = "Browse";
      btnBrowse.UseVisualStyleBackColor = true;
      // 
      // progressBar
      // 
      progressBar.Location = new Point(89, 268);
      progressBar.Name = "progressBar";
      progressBar.Size = new Size(560, 23);
      progressBar.TabIndex = 16;
      // 
      // lblProgress
      // 
      lblProgress.AutoSize = true;
      lblProgress.Font = new Font("Segoe UI", 12F);
      lblProgress.Location = new Point(12, 268);
      lblProgress.Name = "lblProgress";
      lblProgress.Size = new Size(71, 21);
      lblProgress.TabIndex = 17;
      lblProgress.Text = "Progress";
      // 
      // lblTimeTook
      // 
      lblTimeTook.AutoSize = true;
      lblTimeTook.Font = new Font("Segoe UI", 12F);
      lblTimeTook.Location = new Point(12, 312);
      lblTimeTook.Name = "lblTimeTook";
      lblTimeTook.Size = new Size(95, 21);
      lblTimeTook.TabIndex = 18;
      lblTimeTook.Text = "Time took: 0";
      // 
      // txtResult
      // 
      txtResult.Font = new Font("Segoe UI", 12F);
      txtResult.Location = new Point(16, 213);
      txtResult.Name = "txtResult";
      txtResult.ReadOnly = true;
      txtResult.Size = new Size(633, 29);
      txtResult.TabIndex = 19;
      // 
      // lblEncryptedText
      // 
      lblEncryptedText.AutoSize = true;
      lblEncryptedText.Font = new Font("Segoe UI", 12F);
      lblEncryptedText.Location = new Point(12, 189);
      lblEncryptedText.Name = "lblEncryptedText";
      lblEncryptedText.Size = new Size(53, 21);
      lblEncryptedText.TabIndex = 20;
      lblEncryptedText.Text = "Result";
      // 
      // btnEncrypt
      // 
      btnEncrypt.Font = new Font("Segoe UI", 12F);
      btnEncrypt.Location = new Point(488, 308);
      btnEncrypt.Name = "btnEncrypt";
      btnEncrypt.Size = new Size(71, 29);
      btnEncrypt.TabIndex = 21;
      btnEncrypt.Text = "Encrypt";
      btnEncrypt.UseVisualStyleBackColor = true;
      // 
      // btnDecrypt
      // 
      btnDecrypt.Font = new Font("Segoe UI", 12F);
      btnDecrypt.Location = new Point(574, 308);
      btnDecrypt.Name = "btnDecrypt";
      btnDecrypt.Size = new Size(75, 29);
      btnDecrypt.TabIndex = 22;
      btnDecrypt.Text = "Decrypt";
      btnDecrypt.UseVisualStyleBackColor = true;
      // 
      // cbPadding
      // 
      cbPadding.DropDownStyle = ComboBoxStyle.DropDownList;
      cbPadding.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      cbPadding.FormattingEnabled = true;
      cbPadding.Items.AddRange(new object[] { "Text", "Hex", "File" });
      cbPadding.Location = new Point(127, 89);
      cbPadding.Name = "cbPadding";
      cbPadding.Size = new Size(135, 29);
      cbPadding.TabIndex = 23;
      // 
      // lblPadding
      // 
      lblPadding.AutoSize = true;
      lblPadding.Font = new Font("Segoe UI", 12F);
      lblPadding.Location = new Point(12, 93);
      lblPadding.Name = "lblPadding";
      lblPadding.Size = new Size(66, 21);
      lblPadding.TabIndex = 24;
      lblPadding.Text = "Padding";
      // 
      // txtImportedKeyName
      // 
      txtImportedKeyName.BorderStyle = BorderStyle.FixedSingle;
      txtImportedKeyName.Enabled = false;
      txtImportedKeyName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      txtImportedKeyName.Location = new Point(488, 89);
      txtImportedKeyName.Name = "txtImportedKeyName";
      txtImportedKeyName.Size = new Size(157, 29);
      txtImportedKeyName.TabIndex = 25;
      // 
      // lblKeySize
      // 
      lblKeySize.AutoSize = true;
      lblKeySize.Font = new Font("Segoe UI", 12F);
      lblKeySize.Location = new Point(12, 146);
      lblKeySize.Name = "lblKeySize";
      lblKeySize.Size = new Size(106, 21);
      lblKeySize.TabIndex = 26;
      lblKeySize.Text = "Key Size In Bit";
      // 
      // cbKeySize
      // 
      cbKeySize.DropDownStyle = ComboBoxStyle.DropDownList;
      cbKeySize.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      cbKeySize.FormattingEnabled = true;
      cbKeySize.Items.AddRange(new object[] { "Text", "Hex", "File" });
      cbKeySize.Location = new Point(127, 143);
      cbKeySize.Name = "cbKeySize";
      cbKeySize.Size = new Size(96, 29);
      cbKeySize.TabIndex = 27;
      // 
      // cbUseMultithreading
      // 
      cbUseMultithreading.AutoSize = true;
      cbUseMultithreading.Font = new Font("Segoe UI", 12F);
      cbUseMultithreading.Location = new Point(513, 142);
      cbUseMultithreading.Name = "cbUseMultithreading";
      cbUseMultithreading.Size = new Size(132, 25);
      cbUseMultithreading.TabIndex = 28;
      cbUseMultithreading.Text = "Multithreading";
      cbUseMultithreading.UseVisualStyleBackColor = true;
      // 
      // RSAForm
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(657, 351);
      Controls.Add(cbUseMultithreading);
      Controls.Add(cbKeySize);
      Controls.Add(lblKeySize);
      Controls.Add(txtImportedKeyName);
      Controls.Add(lblPadding);
      Controls.Add(cbPadding);
      Controls.Add(btnDecrypt);
      Controls.Add(btnEncrypt);
      Controls.Add(lblEncryptedText);
      Controls.Add(txtResult);
      Controls.Add(lblTimeTook);
      Controls.Add(lblProgress);
      Controls.Add(progressBar);
      Controls.Add(btnBrowse);
      Controls.Add(btnGenerateKey);
      Controls.Add(btnImportKey);
      Controls.Add(cbDataFormat);
      Controls.Add(txtDataOrFilePath);
      Controls.Add(lblDataOrFilePath);
      Controls.Add(lblDataFormat);
      FormBorderStyle = FormBorderStyle.Fixed3D;
      Icon = (Icon)resources.GetObject("$this.Icon");
      MaximizeBox = false;
      Name = "RSAForm";
      Text = "RSA";
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private ComboBox cbDataFormat;
    private TextBox txtDataOrFilePath;
    private Label lblDataOrFilePath;
    private Label lblDataFormat;
    private Button btnImportKey;
    private Button btnGenerateKey;
    private Button btnBrowse;
    private ProgressBar progressBar;
    private Label lblProgress;
    private Label lblTimeTook;
    private TextBox txtResult;
    private Label lblEncryptedText;
    private Button btnEncrypt;
    private Button btnDecrypt;
    private ComboBox cbPadding;
    private Label lblPadding;
    private TextBox txtImportedKeyName;
    private Label lblKeySize;
    private ComboBox cbKeySize;
    private System.Windows.Forms.Timer timer;
    private CheckBox cbUseMultithreading;
  }
}

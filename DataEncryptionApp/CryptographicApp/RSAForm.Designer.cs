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
      btnImportPublicKey = new Button();
      btnGenerateRSAKey = new Button();
      btnBrowse = new Button();
      progressBar = new ProgressBar();
      lblTimeTook = new Label();
      txtResult = new TextBox();
      lblEncryptedText = new Label();
      btnEncrypt = new Button();
      btnDecrypt = new Button();
      cbRSAPadding = new ComboBox();
      lblRSAPadding = new Label();
      txtImportedPublicKeyName = new TextBox();
      lblRSAKeySize = new Label();
      cbRSAKeySize = new ComboBox();
      timer = new System.Windows.Forms.Timer(components);
      cbUseMultithreading = new CheckBox();
      btnHybridEncrypt = new Button();
      btnHybridDecrypt = new Button();
      cbAESKeySize = new ComboBox();
      lblAESKeySize = new Label();
      btnGenerateAESKey = new Button();
      btnImportAESKey = new Button();
      txtImportedAESKeyName = new TextBox();
      lblAESPadding = new Label();
      cbAESPadding = new ComboBox();
      lblHashAlgorithm = new Label();
      cbHashAlgorithm = new ComboBox();
      btnImportPrivateKey = new Button();
      label1 = new Label();
      txtImportedPrivateKeyName = new TextBox();
      label2 = new Label();
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
      // btnImportPublicKey
      // 
      btnImportPublicKey.Font = new Font("Segoe UI", 12F);
      btnImportPublicKey.Location = new Point(387, 89);
      btnImportPublicKey.Name = "btnImportPublicKey";
      btnImportPublicKey.Size = new Size(95, 29);
      btnImportPublicKey.TabIndex = 13;
      btnImportPublicKey.Text = "Import Key";
      btnImportPublicKey.UseVisualStyleBackColor = true;
      // 
      // btnGenerateRSAKey
      // 
      btnGenerateRSAKey.Font = new Font("Segoe UI", 12F);
      btnGenerateRSAKey.Location = new Point(268, 89);
      btnGenerateRSAKey.Name = "btnGenerateRSAKey";
      btnGenerateRSAKey.Size = new Size(113, 29);
      btnGenerateRSAKey.TabIndex = 14;
      btnGenerateRSAKey.Text = "Generate Key";
      btnGenerateRSAKey.UseVisualStyleBackColor = true;
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
      progressBar.Location = new Point(12, 364);
      progressBar.Name = "progressBar";
      progressBar.Size = new Size(633, 23);
      progressBar.TabIndex = 16;
      // 
      // lblTimeTook
      // 
      lblTimeTook.AutoSize = true;
      lblTimeTook.Font = new Font("Segoe UI", 12F);
      lblTimeTook.Location = new Point(8, 408);
      lblTimeTook.Name = "lblTimeTook";
      lblTimeTook.Size = new Size(95, 21);
      lblTimeTook.TabIndex = 18;
      lblTimeTook.Text = "Time took: 0";
      // 
      // txtResult
      // 
      txtResult.Font = new Font("Segoe UI", 12F);
      txtResult.Location = new Point(12, 309);
      txtResult.Name = "txtResult";
      txtResult.ReadOnly = true;
      txtResult.Size = new Size(633, 29);
      txtResult.TabIndex = 19;
      // 
      // lblEncryptedText
      // 
      lblEncryptedText.AutoSize = true;
      lblEncryptedText.Font = new Font("Segoe UI", 12F);
      lblEncryptedText.Location = new Point(21, 292);
      lblEncryptedText.Name = "lblEncryptedText";
      lblEncryptedText.Size = new Size(53, 21);
      lblEncryptedText.TabIndex = 20;
      lblEncryptedText.Text = "Result";
      // 
      // btnEncrypt
      // 
      btnEncrypt.Font = new Font("Segoe UI", 12F);
      btnEncrypt.Location = new Point(484, 404);
      btnEncrypt.Name = "btnEncrypt";
      btnEncrypt.Size = new Size(71, 29);
      btnEncrypt.TabIndex = 21;
      btnEncrypt.Text = "Encrypt";
      btnEncrypt.UseVisualStyleBackColor = true;
      // 
      // btnDecrypt
      // 
      btnDecrypt.Font = new Font("Segoe UI", 12F);
      btnDecrypt.Location = new Point(570, 404);
      btnDecrypt.Name = "btnDecrypt";
      btnDecrypt.Size = new Size(75, 29);
      btnDecrypt.TabIndex = 22;
      btnDecrypt.Text = "Decrypt";
      btnDecrypt.UseVisualStyleBackColor = true;
      // 
      // cbRSAPadding
      // 
      cbRSAPadding.DropDownStyle = ComboBoxStyle.DropDownList;
      cbRSAPadding.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      cbRSAPadding.FormattingEnabled = true;
      cbRSAPadding.Items.AddRange(new object[] { "Text", "Hex", "File" });
      cbRSAPadding.Location = new Point(128, 140);
      cbRSAPadding.Name = "cbRSAPadding";
      cbRSAPadding.Size = new Size(135, 29);
      cbRSAPadding.TabIndex = 23;
      // 
      // lblRSAPadding
      // 
      lblRSAPadding.AutoSize = true;
      lblRSAPadding.Font = new Font("Segoe UI", 12F);
      lblRSAPadding.Location = new Point(12, 143);
      lblRSAPadding.Name = "lblRSAPadding";
      lblRSAPadding.Size = new Size(99, 21);
      lblRSAPadding.TabIndex = 24;
      lblRSAPadding.Text = "RSA Padding";
      // 
      // txtImportedPublicKeyName
      // 
      txtImportedPublicKeyName.BorderStyle = BorderStyle.FixedSingle;
      txtImportedPublicKeyName.Enabled = false;
      txtImportedPublicKeyName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      txtImportedPublicKeyName.Location = new Point(488, 89);
      txtImportedPublicKeyName.Name = "txtImportedPublicKeyName";
      txtImportedPublicKeyName.Size = new Size(157, 29);
      txtImportedPublicKeyName.TabIndex = 25;
      // 
      // lblRSAKeySize
      // 
      lblRSAKeySize.AutoSize = true;
      lblRSAKeySize.Font = new Font("Segoe UI", 12F);
      lblRSAKeySize.Location = new Point(12, 93);
      lblRSAKeySize.Name = "lblRSAKeySize";
      lblRSAKeySize.Size = new Size(100, 21);
      lblRSAKeySize.TabIndex = 26;
      lblRSAKeySize.Text = "RSA Key Size";
      // 
      // cbRSAKeySize
      // 
      cbRSAKeySize.DropDownStyle = ComboBoxStyle.DropDownList;
      cbRSAKeySize.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      cbRSAKeySize.FormattingEnabled = true;
      cbRSAKeySize.Items.AddRange(new object[] { "Text", "Hex", "File" });
      cbRSAKeySize.Location = new Point(128, 88);
      cbRSAKeySize.Name = "cbRSAKeySize";
      cbRSAKeySize.Size = new Size(134, 29);
      cbRSAKeySize.TabIndex = 27;
      // 
      // cbUseMultithreading
      // 
      cbUseMultithreading.AutoSize = true;
      cbUseMultithreading.Font = new Font("Segoe UI", 12F);
      cbUseMultithreading.Location = new Point(513, 283);
      cbUseMultithreading.Name = "cbUseMultithreading";
      cbUseMultithreading.Size = new Size(132, 25);
      cbUseMultithreading.TabIndex = 28;
      cbUseMultithreading.Text = "Multithreading";
      cbUseMultithreading.UseVisualStyleBackColor = true;
      // 
      // btnHybridEncrypt
      // 
      btnHybridEncrypt.Font = new Font("Segoe UI", 12F);
      btnHybridEncrypt.Location = new Point(203, 404);
      btnHybridEncrypt.Name = "btnHybridEncrypt";
      btnHybridEncrypt.Size = new Size(122, 29);
      btnHybridEncrypt.TabIndex = 29;
      btnHybridEncrypt.Text = "Hybrid Encrypt";
      btnHybridEncrypt.UseVisualStyleBackColor = true;
      // 
      // btnHybridDecrypt
      // 
      btnHybridDecrypt.Font = new Font("Segoe UI", 12F);
      btnHybridDecrypt.Location = new Point(340, 404);
      btnHybridDecrypt.Name = "btnHybridDecrypt";
      btnHybridDecrypt.Size = new Size(128, 29);
      btnHybridDecrypt.TabIndex = 30;
      btnHybridDecrypt.Text = "Hybrid Decrypt";
      btnHybridDecrypt.UseVisualStyleBackColor = true;
      // 
      // cbAESKeySize
      // 
      cbAESKeySize.DropDownStyle = ComboBoxStyle.DropDownList;
      cbAESKeySize.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      cbAESKeySize.FormattingEnabled = true;
      cbAESKeySize.Items.AddRange(new object[] { "Text", "Hex", "File" });
      cbAESKeySize.Location = new Point(127, 193);
      cbAESKeySize.Name = "cbAESKeySize";
      cbAESKeySize.Size = new Size(136, 29);
      cbAESKeySize.TabIndex = 31;
      // 
      // lblAESKeySize
      // 
      lblAESKeySize.AutoSize = true;
      lblAESKeySize.Font = new Font("Segoe UI", 12F);
      lblAESKeySize.Location = new Point(12, 196);
      lblAESKeySize.Name = "lblAESKeySize";
      lblAESKeySize.Size = new Size(98, 21);
      lblAESKeySize.TabIndex = 32;
      lblAESKeySize.Text = "AES Key Size";
      // 
      // btnGenerateAESKey
      // 
      btnGenerateAESKey.Font = new Font("Segoe UI", 12F);
      btnGenerateAESKey.Location = new Point(269, 193);
      btnGenerateAESKey.Name = "btnGenerateAESKey";
      btnGenerateAESKey.Size = new Size(113, 29);
      btnGenerateAESKey.TabIndex = 33;
      btnGenerateAESKey.Text = "Generate Key";
      btnGenerateAESKey.UseVisualStyleBackColor = true;
      // 
      // btnImportAESKey
      // 
      btnImportAESKey.Font = new Font("Segoe UI", 12F);
      btnImportAESKey.Location = new Point(388, 193);
      btnImportAESKey.Name = "btnImportAESKey";
      btnImportAESKey.Size = new Size(95, 29);
      btnImportAESKey.TabIndex = 34;
      btnImportAESKey.Text = "Import Key";
      btnImportAESKey.UseVisualStyleBackColor = true;
      // 
      // txtImportedAESKeyName
      // 
      txtImportedAESKeyName.BorderStyle = BorderStyle.FixedSingle;
      txtImportedAESKeyName.Enabled = false;
      txtImportedAESKeyName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      txtImportedAESKeyName.Location = new Point(488, 193);
      txtImportedAESKeyName.Name = "txtImportedAESKeyName";
      txtImportedAESKeyName.Size = new Size(157, 29);
      txtImportedAESKeyName.TabIndex = 35;
      // 
      // lblAESPadding
      // 
      lblAESPadding.AutoSize = true;
      lblAESPadding.Font = new Font("Segoe UI", 12F);
      lblAESPadding.Location = new Point(12, 251);
      lblAESPadding.Name = "lblAESPadding";
      lblAESPadding.Size = new Size(97, 21);
      lblAESPadding.TabIndex = 36;
      lblAESPadding.Text = "AES Padding";
      // 
      // cbAESPadding
      // 
      cbAESPadding.DropDownStyle = ComboBoxStyle.DropDownList;
      cbAESPadding.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      cbAESPadding.FormattingEnabled = true;
      cbAESPadding.Items.AddRange(new object[] { "Text", "Hex", "File" });
      cbAESPadding.Location = new Point(127, 248);
      cbAESPadding.Name = "cbAESPadding";
      cbAESPadding.Size = new Size(135, 29);
      cbAESPadding.TabIndex = 37;
      // 
      // lblHashAlgorithm
      // 
      lblHashAlgorithm.AutoSize = true;
      lblHashAlgorithm.Font = new Font("Segoe UI", 12F);
      lblHashAlgorithm.Location = new Point(363, 251);
      lblHashAlgorithm.Name = "lblHashAlgorithm";
      lblHashAlgorithm.Size = new Size(119, 21);
      lblHashAlgorithm.TabIndex = 38;
      lblHashAlgorithm.Text = "Hash Algorithm";
      // 
      // cbHashAlgorithm
      // 
      cbHashAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
      cbHashAlgorithm.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      cbHashAlgorithm.FormattingEnabled = true;
      cbHashAlgorithm.Items.AddRange(new object[] { "Text", "Hex", "File" });
      cbHashAlgorithm.Location = new Point(488, 248);
      cbHashAlgorithm.Name = "cbHashAlgorithm";
      cbHashAlgorithm.Size = new Size(157, 29);
      cbHashAlgorithm.TabIndex = 39;
      // 
      // btnImportPrivateKey
      // 
      btnImportPrivateKey.Font = new Font("Segoe UI", 12F);
      btnImportPrivateKey.Location = new Point(388, 139);
      btnImportPrivateKey.Name = "btnImportPrivateKey";
      btnImportPrivateKey.Size = new Size(95, 29);
      btnImportPrivateKey.TabIndex = 40;
      btnImportPrivateKey.Text = "Import Key";
      btnImportPrivateKey.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(488, 74);
      label1.Name = "label1";
      label1.Size = new Size(101, 15);
      label1.TabIndex = 41;
      label1.Text = "Import Public Key";
      // 
      // txtImportedPrivateKeyName
      // 
      txtImportedPrivateKeyName.BorderStyle = BorderStyle.FixedSingle;
      txtImportedPrivateKeyName.Enabled = false;
      txtImportedPrivateKeyName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      txtImportedPrivateKeyName.Location = new Point(488, 140);
      txtImportedPrivateKeyName.Name = "txtImportedPrivateKeyName";
      txtImportedPrivateKeyName.Size = new Size(157, 29);
      txtImportedPrivateKeyName.TabIndex = 42;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(488, 122);
      label2.Name = "label2";
      label2.Size = new Size(104, 15);
      label2.TabIndex = 43;
      label2.Text = "Import Private Key";
      // 
      // RSAForm
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(657, 445);
      Controls.Add(label2);
      Controls.Add(txtImportedPrivateKeyName);
      Controls.Add(label1);
      Controls.Add(btnImportPrivateKey);
      Controls.Add(cbHashAlgorithm);
      Controls.Add(lblHashAlgorithm);
      Controls.Add(cbAESPadding);
      Controls.Add(lblAESPadding);
      Controls.Add(txtImportedAESKeyName);
      Controls.Add(btnImportAESKey);
      Controls.Add(btnGenerateAESKey);
      Controls.Add(lblAESKeySize);
      Controls.Add(cbAESKeySize);
      Controls.Add(btnHybridDecrypt);
      Controls.Add(btnHybridEncrypt);
      Controls.Add(cbUseMultithreading);
      Controls.Add(cbRSAKeySize);
      Controls.Add(lblRSAKeySize);
      Controls.Add(txtImportedPublicKeyName);
      Controls.Add(lblRSAPadding);
      Controls.Add(cbRSAPadding);
      Controls.Add(btnDecrypt);
      Controls.Add(btnEncrypt);
      Controls.Add(lblEncryptedText);
      Controls.Add(txtResult);
      Controls.Add(lblTimeTook);
      Controls.Add(progressBar);
      Controls.Add(btnBrowse);
      Controls.Add(btnGenerateRSAKey);
      Controls.Add(btnImportPublicKey);
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
    private Button btnImportPublicKey;
    private Button btnGenerateRSAKey;
    private Button btnBrowse;
    private ProgressBar progressBar;
    private Label lblTimeTook;
    private TextBox txtResult;
    private Label lblEncryptedText;
    private Button btnEncrypt;
    private Button btnDecrypt;
    private ComboBox cbRSAPadding;
    private Label lblRSAPadding;
    private TextBox txtImportedPublicKeyName;
    private Label lblRSAKeySize;
    private ComboBox cbRSAKeySize;
    private System.Windows.Forms.Timer timer;
    private CheckBox cbUseMultithreading;
    private Button btnHybridEncrypt;
    private Button btnHybridDecrypt;
    private ComboBox cbAESKeySize;
    private Label lblAESKeySize;
    private Button btnGenerateAESKey;
    private Button btnImportAESKey;
    private TextBox txtImportedAESKeyName;
    private Label lblAESPadding;
    private ComboBox cbAESPadding;
    private Label lblHashAlgorithm;
    private ComboBox cbHashAlgorithm;
    private Button btnImportPrivateKey;
    private Label label1;
    private TextBox txtImportedPrivateKeyName;
    private Label label2;
  }
}

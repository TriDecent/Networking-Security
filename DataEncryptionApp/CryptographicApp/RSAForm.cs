using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CryptographicApp;

public partial class RSAForm : Form
{
  private readonly Button _btnBrowse, _btnGenerateKey, _btnImportKey;
  private readonly Button _btnEncrypt, _btnDecrypt;
  private readonly ComboBox _cbDataFormat, _cbPadding, _cbKeySize;
  private readonly TextBox _txtDataOrFilePath, _txtResult, _txtImportedKeyName;

  private string _importedKeyFilePath = "";

  public RSAForm()
  {
    InitializeComponent();

    _btnBrowse = btnBrowse;
    _btnGenerateKey = btnGenerateKey;
    _btnImportKey = btnImportKey;
    _btnEncrypt = btnEncrypt;
    _btnDecrypt = btnDecrypt;
    _cbDataFormat = cbDataFormat;
    _cbPadding = cbPadding;
    _cbKeySize = cbKeySize;
    _txtDataOrFilePath = txtDataOrFilePath;
    _txtResult = txtResult;
    _txtImportedKeyName = txtImportedKeyName;

    _cbDataFormat.DataSource = Enum.GetValues<DataFormat>();
    _cbPadding.DataSource = new[]
    {
      RSAEncryptionPadding.Pkcs1, RSAEncryptionPadding.OaepSHA1,
      RSAEncryptionPadding.OaepSHA256, RSAEncryptionPadding.OaepSHA384,
      RSAEncryptionPadding.OaepSHA512,
    };
    _cbKeySize.DataSource = new[] { 1024, 2048, 3072, 4096 };

    _btnGenerateKey.Click += (s, e) => OnGenerateKeyClicked();
    _btnImportKey.Click += (s, e) => OnImportKeyClicked();
    _btnEncrypt.Click += (s, e) => OnEncryptClicked();
  }

  private void OnGenerateKeyClicked()
  {
    var padding = (RSAEncryptionPadding)_cbPadding.SelectedItem!;
    var keySize = (int)_cbKeySize.SelectedItem!;

    var rsa = RSA.Create(keySize);
    var rsaEncryption = new RSAEncryption(rsa, padding);
    var (PublicKey, PrivateKey) = rsaEncryption.GenerateKey();

    SaveKeyPairs(PublicKey, PrivateKey);
  }

  private void OnImportKeyClicked()
  {
    var openDialog = new OpenFileDialog()
    {
      Filter = "PEM files (*.pem)|*.pem|All files (*.*)|*.*"
    };

    if (openDialog.ShowDialog() == DialogResult.OK)
    {
      var filePath = openDialog.FileName;
      _importedKeyFilePath = filePath;

      var fileName = filePath.Split("\\", StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
      _txtImportedKeyName.Text = fileName;
    }
  }

  private void OnEncryptClicked()
  {
    if (string.IsNullOrEmpty(_txtDataOrFilePath.Text)) return;
    
    var dataFormat = (DataFormat)_cbDataFormat.SelectedItem!;
    var padding = (RSAEncryptionPadding)_cbPadding.SelectedItem!;
    var keySize = (int)_cbKeySize.SelectedItem!;

    var rsa = RSA.Create(keySize);
    var publicKeyPem = File.ReadAllText(_importedKeyFilePath);

    var rsaEncryption = new RSAEncryption(rsa, padding);
    var encryptedData = rsaEncryption.Encrypt(_txtDataOrFilePath.Text, publicKeyPem, dataFormat);

    _txtResult.Text = encryptedData;
  }


  [GeneratedRegex(@"public_key(\d+)\.pem")]
  private static partial Regex KeyNumberPattern();

  private static void SaveKeyPairs(string publicKey, string privateKey)
  {
    var baseDir = Path.GetDirectoryName(Application.ExecutablePath)!;
    var directory = Path.Combine(baseDir, "KeyPairs");
    Directory.CreateDirectory(directory);

    var files = Directory.GetFiles(directory, "public_key*.pem");
    int highestNumber = 0;

    foreach (var file in files)
    {
      var match = KeyNumberPattern().Match(file);
      if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
      {
        highestNumber = Math.Max(highestNumber, number);
      }
    }

    var newNumber = highestNumber + 1;
    string publicKeyPath = Path.Combine(directory, $"public_key{newNumber}.pem");
    string privateKeyPath = Path.Combine(directory, $"private_key{newNumber}.pem");

    File.WriteAllText(publicKeyPath, publicKey);
    File.WriteAllText(privateKeyPath, privateKey);
  }
}

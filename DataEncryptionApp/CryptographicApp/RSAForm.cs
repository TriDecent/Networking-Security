using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CryptographicApp;

public partial class RSAForm : Form
{
  private readonly Button _btnBrowse, _btnGenerateKey, _btnImportKey;
  private readonly Button _btnEncrypt, _btnDecrypt;
  private readonly ComboBox _cbDataFormat, _cbPadding, _cbKeySize;
  private readonly TextBox _txtDataOrFilePath, _txtResult, _txtImportedKeyName;
  private readonly Label _lblTimeTook;
  private readonly ProgressBar _progressBar;
  private readonly Stopwatch _stopWatch = new();

  private string _importedKeyFilePath = "";

  private DataFormat _selectedDataFormat = DataFormat.Text;
  private RSAEncryptionPadding _selectedPadding = RSAEncryptionPadding.Pkcs1;
  private int _selectedKeySize = 1024;

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
    _lblTimeTook = lblTimeTook;
    _progressBar = progressBar;

    _cbDataFormat.DataSource = Enum.GetValues<DataFormat>();
    _cbPadding.DataSource = new[]
    {
      RSAEncryptionPadding.Pkcs1, RSAEncryptionPadding.OaepSHA1,
      RSAEncryptionPadding.OaepSHA256, RSAEncryptionPadding.OaepSHA384,
      RSAEncryptionPadding.OaepSHA512,
    };
    _cbKeySize.DataSource = new[] { 1024, 2048, 3072, 4096 };

    _cbDataFormat.SelectedValueChanged += (s, e) => OnSelectedDataFormatChanged();
    _cbPadding.SelectedValueChanged += (s, e) => OnSelectedPaddingChanged();
    _cbKeySize.SelectedValueChanged += (s, e) => OnSelectedKeySizeChanged();

    _btnGenerateKey.Click += (s, e) => OnEncryptOrDecryptStart();
    _btnDecrypt.Click += (s, e) => OnEncryptOrDecryptStart();
    _btnEncrypt.Click += (s, e) => OnEncryptOrDecryptStart();

    _btnGenerateKey.Click += (s, e) => OnGenerateKeyClicked();
    _btnImportKey.Click += (s, e) => OnImportKeyClicked();
    _btnEncrypt.Click += (s, e) => OnEncryptClicked();
    _btnDecrypt.Click += (s, e) => OnDecryptClicked();
    _btnBrowse.Click += (s, e) => OnBrowseClicked();
  }

  [GeneratedRegex(@"public_key(\d+)\.pem")]
  private static partial Regex KeyNumberPattern();

  private static void SaveKeyPairs(string publicKey, string privateKey)
  {
    var directory = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "KeyPairs");
    Directory.CreateDirectory(directory);

    var newNumber = Directory.GetFiles(directory, "public_key*.pem")
      .Select(file => KeyNumberPattern().Match(file))
      .Where(match => match.Success && int.TryParse(match.Groups[1].Value, out _))
      .Select(match => int.Parse(match.Groups[1].Value))
      .DefaultIfEmpty(0)
      .Max() + 1;

    SecureKeyStorage.Write(Path.Combine(directory, $"public_key{newNumber}.pem"), publicKey);
    SecureKeyStorage.Write(Path.Combine(directory, $"private_key{newNumber}.pem"), privateKey);
  }

  private static void HandleFileEncryption(RSAEncryption rsa, string publicKeyPem, string filePath)
  {
    var encryptedDirectory = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "EncryptedFiles");
    Directory.CreateDirectory(encryptedDirectory);
    var encryptedFilePath = Path.Combine(encryptedDirectory,
      $"{Path.GetFileNameWithoutExtension(filePath)}-encrypted{Path.GetExtension(filePath)}");

    rsa.EncryptFile(filePath, encryptedFilePath, publicKeyPem);
  }

  private static void HandleCryptographicException(CryptographicException ex)
  {
    var errorMessage = ex.HResult switch
    {
      unchecked((int)0x8009000D) => "Invalid key format or corrupted key file",
      unchecked((int)0x80090005) => "Key size mismatch or invalid padding",
      _ => $"Encryption error: {ex.Message}\nError code: 0x{ex.HResult:X8}"
    };

    ShowErrorMessage(errorMessage);
  }

  private void OnSelectedDataFormatChanged()
  {
    var selectedFormat = (DataFormat)_cbDataFormat.SelectedItem!;

    if (selectedFormat is DataFormat.File)
    {
      ShowWarningMessage("Ensure correct options for the keys used when working with files.");
    }

    _selectedDataFormat = selectedFormat;
  }


  private void OnSelectedPaddingChanged()
    => _selectedPadding = (RSAEncryptionPadding)_cbPadding.SelectedItem!;

  private void OnSelectedKeySizeChanged()
    => _selectedKeySize = (int)_cbKeySize.SelectedItem!;

  private void OnEncryptOrDecryptStart()
  {
    _stopWatch.Restart();
    _progressBar.Value = 0;

    var rsa = new RSAEncryption(RSA.Create(), _selectedPadding);
  }

  private void OnBrowseClicked()
  {
    var openFileDialog = new OpenFileDialog
    {
      Filter = "All files (*.*)|*.*",
    };

    if (openFileDialog.ShowDialog() == DialogResult.OK)
    {
      _txtDataOrFilePath.Text = openFileDialog.FileName;
    }
  }

  private void OnGenerateKeyClicked()
  {
    var rsa = RSA.Create(_selectedKeySize);
    var rsaEncryption = new RSAEncryption(rsa, _selectedPadding);
    var (publicKey, privateKey) = rsaEncryption.GenerateKey();

    SaveKeyPairs(publicKey, privateKey);

    FinalizeProcess(null);

    ShowSuccessMessage("Keys have been successfully generated and saved in the KeyPairs folder.");
  }

  private void OnImportKeyClicked()
  {
    using var openDialog = new OpenFileDialog
    {
      Filter = "PEM files (*.pem)|*.pem|All files (*.*)|*.*"
    };

    if (openDialog.ShowDialog() == DialogResult.OK)
    {
      _importedKeyFilePath = openDialog.FileName;
      _txtImportedKeyName.Text = Path.GetFileName(_importedKeyFilePath);
    }
  }

  private void OnEncryptClicked()
  {
    if (string.IsNullOrEmpty(_txtDataOrFilePath.Text) || string.IsNullOrEmpty(_txtImportedKeyName.Text))
    {
      ShowErrorMessage("Data or file path and imported key name cannot be empty.");
      return;
    }

    var publicKeyPem = SecureKeyStorage.Read(_importedKeyFilePath);
    var rsa = RSA.Create();
    var rsaEncryption = new RSAEncryption(rsa, _selectedPadding);

    ToggleButton(_btnEncrypt);
    try
    {
      if (_selectedDataFormat != DataFormat.File)
      {
        HandleStringEncryption(rsaEncryption, publicKeyPem, _txtDataOrFilePath.Text);
        FinalizeProcess(_btnEncrypt);
        return;
      }

      HandleFileEncryption(rsaEncryption, publicKeyPem, _txtDataOrFilePath.Text);
      FinalizeProcess(_btnEncrypt);
      ShowSuccessMessage("Your encrypted file has been saved to EncryptedFiles folder");
    }
    catch (CryptographicException ex)
    {
      FinalizeProcess(_btnEncrypt);
      HandleCryptographicException(ex);
    }
    catch (Exception ex)
    {
      FinalizeProcess(_btnEncrypt);
      ShowErrorMessage(ex.Message);
    }
  }

  private void HandleStringEncryption(RSAEncryption rsa, string publicKeyPem, string dataString)
  {
    if (_selectedDataFormat == DataFormat.Text)
    {
      HandleTextEncryption(rsa, publicKeyPem, dataString);
      return;
    }

    if (!dataString.IsHexString())
    {
      ShowErrorMessage("The provided data is not a valid hexadecimal string.");
      return;
    }

    HandleHexEncryption(rsa, publicKeyPem, dataString);
  }

  private void HandleTextEncryption(RSAEncryption rsa, string publicKeyPem, string text)
    => _txtResult.Text = rsa.Encrypt(text, publicKeyPem, _selectedDataFormat);

  private void HandleHexEncryption(RSAEncryption rsa, string publicKeyPem, string hex)
    => _txtResult.Text = rsa.Encrypt(hex, publicKeyPem, _selectedDataFormat);

  private void OnDecryptClicked()
  {
    if (string.IsNullOrEmpty(_txtDataOrFilePath.Text) || string.IsNullOrEmpty(_txtImportedKeyName.Text))
    {
      ShowErrorMessage("Data or file path and imported key name cannot be empty.");
      return;
    }

    var privateKeyPem = SecureKeyStorage.Read(_importedKeyFilePath);
    var rsa = RSA.Create();
    var rsaEncryption = new RSAEncryption(rsa, _selectedPadding);

    ToggleButton(_btnDecrypt);
    try
    {
      if (_selectedDataFormat != DataFormat.File)
      {
        HandleStringDecryption(rsaEncryption, privateKeyPem, _txtDataOrFilePath.Text);
        FinalizeProcess(_btnDecrypt);
        return;
      }

      HandleFileDecryption(rsaEncryption, privateKeyPem, _txtDataOrFilePath.Text);
      FinalizeProcess(_btnDecrypt);
      ShowSuccessMessage("Your decrypted file has been saved to DecryptedFiles folder");
    }
    catch (CryptographicException ex)
    {
      FinalizeProcess(_btnDecrypt);
      HandleCryptographicException(ex);
    }
    catch (Exception ex)
    {
      FinalizeProcess(_btnDecrypt);
      ShowErrorMessage(ex.Message);
    }
  }

  private void HandleStringDecryption(RSAEncryption rsa, string privateKeyPem, string encryptedData)
  {
    if (_selectedDataFormat == DataFormat.Text)
    {
      HandleTextDecryption(rsa, privateKeyPem, encryptedData);
      return;
    }

    if (!encryptedData.IsHexString())
    {
      ShowErrorMessage("The provided data is not a valid hexadecimal string.");
      return;
    }

    HandleHexDecryption(rsa, privateKeyPem, encryptedData);
  }

  private void HandleTextDecryption(RSAEncryption rsa, string privateKeyPem, string encryptedText)
    => _txtResult.Text = rsa.Decrypt(encryptedText, privateKeyPem, _selectedDataFormat);

  private void HandleHexDecryption(RSAEncryption rsa, string privateKeyPem, string encryptedHex)
    => _txtResult.Text = rsa.Decrypt(encryptedHex, privateKeyPem, _selectedDataFormat);

  private static void HandleFileDecryption(RSAEncryption rsa, string privateKeyPem, string filePath)
  {
    var decryptedDirectory = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "DecryptedFiles");
    Directory.CreateDirectory(decryptedDirectory);
    var decryptedFilePath = Path.Combine(decryptedDirectory,
      $"{Path.GetFileNameWithoutExtension(filePath)}-decrypted{Path.GetExtension(filePath)}");
    rsa.DecryptFile(filePath, decryptedFilePath, privateKeyPem);
  }

  private void FinalizeProcess(Button? button)
  {
    _stopWatch.Stop();
    _progressBar.Value = 100;
    DisplayTimeTook(_stopWatch.ElapsedMilliseconds);
    if (button is not null) ToggleButton(button);
  }

  private void DisplayTimeTook(long elapsedMilliseconds)
    => _lblTimeTook.Text = $"Time took: {elapsedMilliseconds} ms";

  private static void ShowWarningMessage(string message) => MessageBox.Show(
    message,
    "Warning",
    MessageBoxButtons.OK,
    MessageBoxIcon.Warning);

  private static void ShowSuccessMessage(string message) => MessageBox.Show(
    message,
    "Success",
    MessageBoxButtons.OK,
    MessageBoxIcon.Information);

  private static void ShowErrorMessage(string message) => MessageBox.Show(
    message,
    "Error",
    MessageBoxButtons.OK,
    MessageBoxIcon.Error);

  private static void ToggleButton(Button button)
    => button.Enabled = !button.Enabled;
}

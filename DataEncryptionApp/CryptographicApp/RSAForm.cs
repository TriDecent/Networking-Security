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
  private readonly CheckBox _cbUseMultithreading;
  private readonly Label _lblTimeTook;
  private readonly ProgressBar _progressBar;
  private readonly Stopwatch _stopWatch = new();

  private readonly SecureKeyStorage _keyStorage = new();

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
    _cbUseMultithreading = cbUseMultithreading;
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

    _btnGenerateKey.Click += async (s, e) => await OnGenerateKeyClickedAsync();
    _btnImportKey.Click += (s, e) => OnImportKeyClicked();
    _btnEncrypt.Click += async (s, e) => await OnEncryptClickedAsync();
    _btnDecrypt.Click += async (s, e) => await OnDecryptClickedAsync();
    _btnBrowse.Click += (s, e) => OnBrowseClicked();
  }

  private void OnSelectedDataFormatChanged()
  {
    var selectedFormat = (DataFormat)_cbDataFormat.SelectedItem!;

    if (selectedFormat is DataFormat.File)
    {
      MessageNotifier.ShowWarning("Ensure correct options for the keys used when working with files.");
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

  private async Task OnGenerateKeyClickedAsync()
  {
    var rsa = RSA.Create(_selectedKeySize);
    var rsaEncryption = new RSAEncryption(rsa, _selectedPadding);

    if (_cbUseMultithreading.Checked)
    {
      await Task.Run(GenerateAndSaveKeys);
    }
    else
    {
      GenerateAndSaveKeys();
    }

    ToggleProgress(false);
    MessageNotifier.ShowSuccess("Keys have been successfully generated and saved in the KeyPairs folder.");

    void GenerateAndSaveKeys()
    {
      var (publicKey, privateKey) = rsaEncryption.GenerateKey();
      _keyStorage.SaveKeyPair(new KeyPair(publicKey, privateKey));
    }
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

  private async Task OnEncryptClickedAsync()
  {
    if (!AreInputsValid())
    {
      MessageNotifier.ShowError("Data or file path and imported key name cannot be empty.");
      return;
    }

    ToggleButton(_btnEncrypt);
    await PerformWithProgress(PerformEncryptionAsync);
    ToggleButton(_btnEncrypt);
  }

  private async Task OnDecryptClickedAsync()
  {
    if (!AreInputsValid())
    {
      MessageNotifier.ShowError("Data or file path and imported key name cannot be empty.");
      return;
    }

    ToggleButton(_btnDecrypt);
    await PerformWithProgress(PerformDecryptionAsync);
    ToggleButton(_btnDecrypt);
  }

  private async Task PerformWithProgress(Func<Task> runCryptographicOperation)
  {
    ToggleProgress(true);

    try
    {
      await runCryptographicOperation();
      ToggleProgress(false);
      if (runCryptographicOperation.Method.Name == nameof(PerformEncryptionAsync))
      {
        MessageNotifier.ShowSuccess("Your encrypted files has been saved to EncryptedFiles folder");
        return;
      }

      MessageNotifier.ShowSuccess("Your decrypted files has been saved to DecryptedFiles folder");
    }
    catch (CryptographicException ex)
    {
      ToggleProgress(false);
      HandleCryptographicException(ex);
    }
    catch (Exception ex)
    {
      ToggleProgress(false);
      MessageNotifier.ShowError($"An error occurred: {ex.Message}");
    }
    finally
    {
      ToggleProgress(false);
    }
  }

  private Task PerformEncryptionAsync()
  {
    var publicKeyPem = _keyStorage.Read(_importedKeyFilePath);
    var rsaEncryption = new RSAEncryption(RSA.Create(), _selectedPadding);

    return _cbUseMultithreading.Checked
      ? Task.Run(() => EncryptData(rsaEncryption, publicKeyPem))
      : Task.CompletedTask.ContinueWith(_ => EncryptData(rsaEncryption, publicKeyPem));
  }

  private Task PerformDecryptionAsync()
  {
    var privateKeyPem = _keyStorage.Read(_importedKeyFilePath);
    var rsaEncryption = new RSAEncryption(RSA.Create(), _selectedPadding);

    return _cbUseMultithreading.Checked
      ? Task.Run(() => DecryptData(rsaEncryption, privateKeyPem))
      : Task.CompletedTask.ContinueWith(_ => DecryptData(rsaEncryption, privateKeyPem));
  }

  private void EncryptData(RSAEncryption rsaEncryption, string publicKeyPem)
  {
    if (_selectedDataFormat == DataFormat.File)
    {
      rsaEncryption.EncryptFile(
        _txtDataOrFilePath.Text,
        GetEncryptedFilePath(_txtDataOrFilePath.Text),
        publicKeyPem);
    }
    else
    {
      _txtResult.Invoke(() =>
        _txtResult.Text = rsaEncryption.Encrypt(
          _txtDataOrFilePath.Text,
          publicKeyPem,
          _selectedDataFormat));
    }
  }

  private void DecryptData(RSAEncryption rsaEncryption, string privateKeyPem)
  {
    if (_selectedDataFormat == DataFormat.File)
    {
      rsaEncryption.DecryptFile(
        _txtDataOrFilePath.Text,
        GetDecryptedFilePath(_txtDataOrFilePath.Text),
        privateKeyPem);
    }
    else
    {
      _txtResult.Invoke(() =>
        _txtResult.Text = rsaEncryption.Decrypt(
          _txtDataOrFilePath.Text,
          privateKeyPem,
          _selectedDataFormat));
    }
  }

  private static string GetEncryptedFilePath(string inputPath)
  {
    var encryptedDirectory = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "EncryptedFiles");
    Directory.CreateDirectory(encryptedDirectory);
    return Path.Combine(encryptedDirectory,
      $"{Path.GetFileNameWithoutExtension(inputPath)}-encrypted{Path.GetExtension(inputPath)}");
  }

  private static string GetDecryptedFilePath(string inputPath)
  {
    var decryptedDirectory = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "DecryptedFiles");
    Directory.CreateDirectory(decryptedDirectory);
    return Path.Combine(decryptedDirectory,
      $"{Path.GetFileNameWithoutExtension(inputPath)}-decrypted{Path.GetExtension(inputPath)}");
  }

  private bool AreInputsValid()
    => !string.IsNullOrEmpty(_txtDataOrFilePath.Text) && !string.IsNullOrEmpty(_txtImportedKeyName.Text);

  private void ToggleProgress(bool isProcessing)
  {
    _progressBar.Value = isProcessing ? 50 : 100;

    if (isProcessing) return;

    _stopWatch.Stop();
    DisplayTimeTook(_stopWatch.ElapsedMilliseconds);
  }

  private static void ToggleButton(Button button)
    => button.Enabled = !button.Enabled;

  private void DisplayTimeTook(long elapsedMilliseconds)
    => _lblTimeTook.Text = $"Time took: {elapsedMilliseconds} ms";

  private static void HandleCryptographicException(CryptographicException ex)
  {
    var errorMessage = ex.HResult switch
    {
      unchecked((int)0x8009000D) => "Invalid key format or corrupted key file",
      unchecked((int)0x80090005) => "Key size mismatch or invalid padding",
      _ => $"Cryptographic error: {ex.Message}\nError code: 0x{ex.HResult:X8}"
    };

    MessageNotifier.ShowError(errorMessage);
  }
}

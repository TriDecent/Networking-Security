using System.Diagnostics;
using System.Security.Cryptography;
using CryptographicApp.CryptographicCores;
using CryptographicApp.CryptographicCores.KeysRepository;
using CryptographicApp.Enums;
using CryptographicApp.Utils;

namespace CryptographicApp;

public partial class RSAForm : Form
{
  private const string EncryptedOutputDirectory = "EncryptedFiles";
  private const string DecryptionOutputDirectory = "DecryptedFiles";
  private readonly Button _btnBrowse, _btnGenerateRSAKey, _btnImportRSAKey;
  private readonly Button _btnRSAEncrypt, _btnRSADecrypt;
  private readonly ComboBox _cbDataFormat, _cbRSAPadding, _cbRSAKeySize;
  private readonly TextBox _txtDataOrFilePath, _txtResult, _txtImportedRSAKeyName;
  private readonly Button _btnHybridEncrypt, _btnHybridDecrypt;
  private readonly Button _btnGenerateAESKey, _btnImportAESKey;
  private readonly ComboBox _cbAESPadding, _cbAESKeySize, _cbHashAlgorithm;
  private readonly CheckBox _cbUseMultithreading;
  private readonly Label _lblTimeTook;
  private readonly ProgressBar _progressBar;
  private readonly Stopwatch _stopWatch = new();

  private readonly SecureKeyStorage _keyStorage = new();

  private string _importedKeyFilePath = "";

  private DataFormat _selectedDataFormat = DataFormat.Text;
  private RSAEncryptionPadding _selectedRSAPadding = RSAEncryptionPadding.Pkcs1;
  private int _selectedRSAKeySize = 1024;

  private int _selectedAESKeySize = 128;
  private PaddingMode _selectedAESPadding = PaddingMode.PKCS7;
  private Enums.HashAlgorithm _selectedHashAlgorithm = Enums.HashAlgorithm.SHA256;

  public RSAForm()
  {
    InitializeComponent();

    _btnBrowse = btnBrowse;
    _btnGenerateRSAKey = btnGenerateRSAKey;
    _btnImportRSAKey = btnImportRSAKey;
    _btnRSAEncrypt = btnEncrypt;
    _btnRSADecrypt = btnDecrypt;
    _cbDataFormat = cbDataFormat;
    _cbRSAPadding = cbRSAPadding;
    _cbRSAKeySize = cbRSAKeySize;
    _txtDataOrFilePath = txtDataOrFilePath;
    _txtResult = txtResult;
    _txtImportedRSAKeyName = txtImportedRSAKeyName;
    _cbUseMultithreading = cbUseMultithreading;
    _btnHybridEncrypt = btnHybridEncrypt;
    _btnHybridDecrypt = btnHybridDecrypt;
    _btnGenerateAESKey = btnGenerateAESKey;
    _btnImportAESKey = btnImportAESKey;
    _cbAESPadding = cbAESPadding;
    _cbAESKeySize = cbAESKeySize;
    _cbHashAlgorithm = cbHashAlgorithm;
    _lblTimeTook = lblTimeTook;
    _progressBar = progressBar;

    _cbDataFormat.DataSource = Enum.GetValues<DataFormat>();
    _cbRSAPadding.DataSource = new[]
    {
      RSAEncryptionPadding.Pkcs1, RSAEncryptionPadding.OaepSHA1,
      RSAEncryptionPadding.OaepSHA256, RSAEncryptionPadding.OaepSHA384,
      RSAEncryptionPadding.OaepSHA512,
    };
    _cbRSAKeySize.DataSource = new[] { 1024, 2048, 3072, 4096 };

    _cbAESPadding.DataSource = new[] {
      PaddingMode.PKCS7, PaddingMode.ISO10126, PaddingMode.ANSIX923,
      PaddingMode.Zeros, PaddingMode.None
    };
    _cbAESKeySize.DataSource = new[] { 128, 192, 256 };
    _cbHashAlgorithm.DataSource = Enum.GetValues<Enums.HashAlgorithm>();

    _cbDataFormat.SelectedValueChanged += (s, e) => OnSelectedDataFormatChanged();
    _cbRSAPadding.SelectedValueChanged += (s, e) => OnSelectedRSAPaddingChanged();
    _cbRSAKeySize.SelectedValueChanged += (s, e) => OnSelectedRSAKeySizeChanged();

    _btnGenerateRSAKey.Click += (s, e) => OnEncryptOrDecryptStart();
    _btnRSADecrypt.Click += (s, e) => OnEncryptOrDecryptStart();
    _btnRSAEncrypt.Click += (s, e) => OnEncryptOrDecryptStart();

    _btnGenerateRSAKey.Click += async (s, e) => await OnGenerateKeyClickedAsync();
    _btnImportRSAKey.Click += (s, e) => OnImportKeyClicked();
    _btnRSAEncrypt.Click += async (s, e) => await OnEncryptClickedAsync();
    _btnRSADecrypt.Click += async (s, e) => await OnDecryptClickedAsync();
    _btnBrowse.Click += (s, e) => OnBrowseClicked();

    _cbAESPadding.SelectedValueChanged += (s, e) => OnSelectedAESPaddingChanged();
    _cbRSAKeySize.SelectedValueChanged += (s, e) => OnSelectedAESKeySizeChanged();
    _cbHashAlgorithm.SelectedValueChanged += (s, e) => OnSelectedHashAlgorithmChanged();
  }

  private void OnSelectedDataFormatChanged()
  {
    var selectedFormat = (DataFormat)_cbDataFormat.SelectedItem!;

    if (selectedFormat is DataFormat.File)
    {
      MessageNotifier.ShowWarning(
        "Ensure correct options for the keys used when working with files.");
    }

    _selectedDataFormat = selectedFormat;
  }

  private void OnSelectedRSAPaddingChanged()
    => _selectedRSAPadding = (RSAEncryptionPadding)_cbRSAPadding.SelectedItem!;

  private void OnSelectedRSAKeySizeChanged()
    => _selectedRSAKeySize = (int)_cbRSAKeySize.SelectedItem!;

  private void OnSelectedAESPaddingChanged()
    => _selectedAESPadding = (PaddingMode)_cbAESKeySize.SelectedItem!;

  private void OnSelectedAESKeySizeChanged()
    => _selectedAESKeySize = (int)_cbAESKeySize.SelectedItem!;

  private void OnSelectedHashAlgorithmChanged() 
    => _selectedHashAlgorithm = (Enums.HashAlgorithm)_cbHashAlgorithm.SelectedItem!;

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
    var rsa = RSA.Create(_selectedRSAKeySize);
    var rsaEncryption = new RSAEncryption(rsa, _selectedRSAPadding);

    if (_cbUseMultithreading.Checked)
    {
      await Task.Run(GenerateAndSaveKeys);
    }
    else
    {
      GenerateAndSaveKeys();
    }

    ToggleProgress(false);
    MessageNotifier.ShowSuccess(
      "Keys have been successfully generated and saved in the KeyPairs folder.");

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
      _txtImportedRSAKeyName.Text = Path.GetFileName(_importedKeyFilePath);
    }
  }

  private async Task OnEncryptClickedAsync()
  {
    if (!AreInputsForRSAValid())
    {
      MessageNotifier.ShowError(
        "Data or file path and imported key name cannot be empty.");
      return;
    }

    ToggleButton(_btnRSAEncrypt);
    await PerformWithProgress(PerformEncryptionAsync);
    ToggleButton(_btnRSAEncrypt);
  }

  private async Task OnDecryptClickedAsync()
  {
    if (!AreInputsForRSAValid())
    {
      MessageNotifier.ShowError(
        "Data or file path and imported key name cannot be empty.");
      return;
    }

    ToggleButton(_btnRSADecrypt);
    await PerformWithProgress(PerformDecryptionAsync);
    ToggleButton(_btnRSADecrypt);
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
        MessageNotifier.ShowSuccess(
          "Your encrypted files has been saved to EncryptedFiles folder");
        return;
      }

      MessageNotifier.ShowSuccess(
        "Your decrypted files has been saved to DecryptedFiles folder");
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
    var rsaEncryption = new RSAEncryption(RSA.Create(), _selectedRSAPadding);

    return _cbUseMultithreading.Checked
      ? Task.Run(() => EncryptData(rsaEncryption, publicKeyPem))
      : Task.CompletedTask.ContinueWith(_ => EncryptData(rsaEncryption, publicKeyPem));
  }

  private Task PerformDecryptionAsync()
  {
    var privateKeyPem = _keyStorage.Read(_importedKeyFilePath);
    var rsaEncryption = new RSAEncryption(RSA.Create(), _selectedRSAPadding);

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
    var encryptedDirectory = Path.Combine(
      Path.GetDirectoryName(Application.ExecutablePath)!, EncryptedOutputDirectory);
    Directory.CreateDirectory(encryptedDirectory);
    return Path.Combine(encryptedDirectory,
      $"{Path.GetFileNameWithoutExtension(inputPath)}-encrypted{Path.GetExtension(inputPath)}");
  }

  private static string GetDecryptedFilePath(string inputPath)
  {
    var decryptedDirectory = Path.Combine(
      Path.GetDirectoryName(Application.ExecutablePath)!, DecryptionOutputDirectory);
    Directory.CreateDirectory(decryptedDirectory);
    return Path.Combine(decryptedDirectory,
      $"{Path.GetFileNameWithoutExtension(inputPath)}-decrypted{Path.GetExtension(inputPath)}");
  }

  private bool AreInputsForRSAValid()
    => !string.IsNullOrEmpty(_txtDataOrFilePath.Text) &&
    !string.IsNullOrEmpty(_txtImportedRSAKeyName.Text);

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

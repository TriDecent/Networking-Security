using CryptographicApp.CryptographicCores.Asymmetric;
using CryptographicApp.CryptographicCores.HashGenerators;
using CryptographicApp.CryptographicCores.Hybrid;
using CryptographicApp.CryptographicCores.IntegrityVerifier;
using CryptographicApp.CryptographicCores.KeysRepository;
using CryptographicApp.CryptographicCores.MetadataHeaderExtractor;
using CryptographicApp.CryptographicCores.Symmetric;
using CryptographicApp.Enums;
using CryptographicApp.Models;
using CryptographicApp.Utils;
using System.Diagnostics;
using System.Security.Cryptography;

namespace CryptographicApp;

public partial class RSAForm : Form
{
  private const string ENCRYPTED_OUTPUT_DIRECTORY = "EncryptedFiles";
  private const string DECRYPTION_OUTPUT_DIRECTORY = "DecryptedFiles";
  private readonly Button _btnBrowse, _btnGenerateRSAKey;
  private readonly Button _btnImportPrivateKey, _btnImportPublicKey;
  private readonly Button _btnRSAEncrypt, _btnRSADecrypt;
  private readonly ComboBox _cbDataFormat, _cbRSAPadding, _cbRSAKeySize;
  private readonly TextBox _txtDataOrFilePath, _txtResult;
  private readonly TextBox _txtImportedPublicKeyName, _txtImportedPrivateKeyName;
  private readonly Button _btnHybridEncrypt, _btnHybridDecrypt;
  private readonly Button _btnGenerateAESKey, _btnImportAESKey;
  private readonly Button _btnCheckIntegrity;
  private readonly TextBox _txtImportedAESKeyName;
  private readonly ComboBox _cbAESPadding, _cbAESKeySize, _cbHashAlgorithm;
  private readonly CheckBox _cbUseMultithreading;
  private readonly Label _lblTimeTook;
  private readonly ProgressBar _progressBar;
  private readonly Stopwatch _stopWatch = new();

  private readonly SecureKeyStorage _keyStorage = new();

  private string _importedPublicKeyFilePath = "";
  private string _importedPrivateKeyFilePath = "";
  private string _importedAESKeyFilePath = "";

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
    _btnImportPublicKey = btnImportPublicKey;
    _btnImportPrivateKey = btnImportPrivateKey;
    _btnRSAEncrypt = btnEncrypt;
    _btnRSADecrypt = btnDecrypt;
    _cbDataFormat = cbDataFormat;
    _cbRSAPadding = cbRSAPadding;
    _cbRSAKeySize = cbRSAKeySize;
    _txtDataOrFilePath = txtDataOrFilePath;
    _txtResult = txtResult;
    _txtImportedPublicKeyName = txtImportedPublicKeyName;
    _txtImportedPrivateKeyName = txtImportedPrivateKeyName;
    _cbUseMultithreading = cbUseMultithreading;
    _btnHybridEncrypt = btnHybridEncrypt;
    _btnHybridDecrypt = btnHybridDecrypt;
    _btnGenerateAESKey = btnGenerateAESKey;
    _btnImportAESKey = btnImportAESKey;
    _btnCheckIntegrity = btnCheckIntegrity;
    _cbAESPadding = cbAESPadding;
    _cbAESKeySize = cbAESKeySize;
    _txtImportedAESKeyName = txtImportedAESKeyName;
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

    _btnGenerateRSAKey.Click += (s, e) => OnProcessStart();
    _btnRSADecrypt.Click += (s, e) => OnProcessStart();
    _btnRSAEncrypt.Click += (s, e) => OnProcessStart();

    _btnGenerateRSAKey.Click += async (s, e) => await OnGenerateRSAKeyClickedAsync();
    _btnImportPublicKey.Click += (s, e) => OnImportPublicKeyClicked();
    _btnImportPrivateKey.Click += (s, e) => OnImportPrivateKeyClicked();
    _btnRSAEncrypt.Click += async (s, e) => await OnRSAEncryptClickedAsync();
    _btnRSADecrypt.Click += async (s, e) => await OnRSADecryptClickedAsync();
    _btnBrowse.Click += (s, e) => OnBrowseClicked();

    _cbAESPadding.SelectedValueChanged += (s, e) => OnSelectedAESPaddingChanged();
    _cbRSAKeySize.SelectedValueChanged += (s, e) => OnSelectedAESKeySizeChanged();
    _cbHashAlgorithm.SelectedValueChanged += (s, e) => OnSelectedHashAlgorithmChanged();

    _btnGenerateAESKey.Click += (s, e) => OnProcessStart();
    _btnHybridEncrypt.Click += (s, e) => OnProcessStart();
    _btnHybridDecrypt.Click += (s, e) => OnProcessStart();

    _btnGenerateAESKey.Click += async (s, e) => await OnGenerateAESKeyClickedAsync();
    _btnImportAESKey.Click += (s, e) => OnImportAESKeyClicked();
    _btnHybridEncrypt.Click += async (s, e) => await OnHybridEncryptClickedAsync();
    _btnHybridDecrypt.Click += async (s, e) => await OnHybridDecryptClickedAsync();

    _btnCheckIntegrity.Click += async (s, e) => await OnIntegrityCheckClicked();
  }

  private async Task OnIntegrityCheckClicked()
  {
    using var rsa = RSA.Create();
    using var aes = Aes.Create();

    var integrityVerifier = CryptoComponentFactory.CreateIntegrityVerifier(
      rsa, aes, _selectedRSAPadding, _selectedHashAlgorithm);

    var rsaKey = LoadRSAKey();

    bool isIntact = false;
    try
    {
      isIntact = await integrityVerifier.Verify(_txtDataOrFilePath.Text, rsaKey);
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

    if (isIntact)
    {
      MessageBox.Show("File integrity verified and intact.");
      return;
    }

    MessageBox.Show("File integrity check failed. Corruption detected..");
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

  private void OnProcessStart()
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

  private async Task OnGenerateKeyClickedAsync<TEncryption, TKey>(Func<TEncryption> createEncryption, Func<TEncryption, TKey> generateKey, Action<TKey> saveKey, string keysFolder)
  {
    if (_cbUseMultithreading.Checked)
    {
      await Task.Run(GenerateAndSaveKeys);
    }
    else
    {
      GenerateAndSaveKeys();
    }

    ToggleProgress(false);
    MessageNotifier.ShowSuccess($"Keys have been successfully generated and saved in the {keysFolder} folder.");

    void GenerateAndSaveKeys()
    {
      var encryption = createEncryption();
      var key = generateKey(encryption);
      saveKey(key);
    }
  }

  private async Task OnGenerateAESKeyClickedAsync()
  {
    using var aes = Aes.Create();

    await OnGenerateKeyClickedAsync(
      createEncryption: () => new AESEncryption(aes),
      generateKey: encryption =>
      {
        encryption.SetKeySize(_selectedAESKeySize);
        var aesKey = encryption.GenerateKey();
        return aesKey;
      },
      saveKey: _keyStorage.SaveAESKey,
      keysFolder: SecureKeyStorage.AES_KEYS_FOLDER
    );
  }

  private async Task OnGenerateRSAKeyClickedAsync()
  {
    using var rsa = RSA.Create();

    await OnGenerateKeyClickedAsync(
      createEncryption: () => new RSAEncryption(rsa, _selectedRSAPadding),
      generateKey: encryption =>
      {
        encryption.SetKeySize(_selectedRSAKeySize);
        var (publicKey, privateKey) = encryption.GenerateKey();
        return new RSAKey(publicKey, privateKey);
      },
      saveKey: _keyStorage.SaveKeyPair,
      keysFolder: SecureKeyStorage.RSA_KEYS_FOLDER
    );
  }

  private void OnImportPublicKeyClicked()
  {
    using var openDialog = new OpenFileDialog
    {
      Filter = "PEM files (*.pem)|*.pem|All files (*.*)|*.*"
    };

    if (openDialog.ShowDialog() == DialogResult.OK)
    {
      _importedPublicKeyFilePath = openDialog.FileName;
      _txtImportedPublicKeyName.Text = Path.GetFileName(_importedPublicKeyFilePath);
    }
  }

  private void OnImportPrivateKeyClicked()
  {
    using var openDialog = new OpenFileDialog
    {
      Filter = "PEM files (*.pem)|*.pem|All files (*.*)|*.*"
    };

    if (openDialog.ShowDialog() == DialogResult.OK)
    {
      _importedPrivateKeyFilePath = openDialog.FileName;
      _txtImportedPrivateKeyName.Text = Path.GetFileName(_importedPrivateKeyFilePath);
    }
  }

  private void OnImportAESKeyClicked()
  {
    using var openDialog = new OpenFileDialog
    {
      Filter = "Key files (*.key)|*.key|All files (*.*)|*.*"
    };

    if (openDialog.ShowDialog() == DialogResult.OK)
    {
      _importedAESKeyFilePath = openDialog.FileName;
      _txtImportedAESKeyName.Text = Path.GetFileName(_importedAESKeyFilePath);
    }
  }

  private async Task OnRSAEncryptClickedAsync()
  {
    if (!AreInputsForRSAValid())
    {
      MessageNotifier.ShowError(
        "Data or file path and imported key cannot be empty.");
      return;
    }

    using var rsa = RSA.Create();
    ToggleButton(_btnRSAEncrypt);
    await PerformWithProgress(() => PerformRSAEncryptionAsync(rsa));
    ToggleButton(_btnRSAEncrypt);
  }

  private async Task OnRSADecryptClickedAsync()
  {
    if (!AreInputsForRSAValid())
    {
      MessageNotifier.ShowError(
        "Data or file path and imported key cannot be empty.");
      return;
    }

    using var rsa = RSA.Create();
    ToggleButton(_btnRSADecrypt);
    await PerformWithProgress(() => PerformRSADecryptionAsync(rsa));
    ToggleButton(_btnRSADecrypt);
  }

  private async Task OnHybridEncryptClickedAsync()
  {
    if (_selectedDataFormat != DataFormat.File)
    {
      MessageNotifier.ShowWarning(
       "Hybrid encryption only work with files.");
      return;
    }

    if (!AreInputsForHybridValid())
    {
      MessageNotifier.ShowError(
        "Data or file path and imported key cannot be empty.");
      return;
    }

    using var rsa = RSA.Create();
    using var aes = Aes.Create();

    ToggleButton(_btnHybridEncrypt);
    // temporary like this since hybrid encryption does not have synchronous method
    await PerformWithProgress(async () =>
      await PerformHybridEncryption(rsa, aes));
    ToggleButton(_btnHybridEncrypt);
  }

  private async Task OnHybridDecryptClickedAsync()
  {
    if (_selectedDataFormat != DataFormat.File)
    {
      MessageNotifier.ShowWarning(
       "Hybrid decryption only work with files.");
      return;
    }

    if (!AreInputsForHybridValid())
    {
      MessageNotifier.ShowError(
        "Data or file path and imported key cannot be empty.");
      return;
    }

    using var rsa = RSA.Create();
    using var aes = Aes.Create();

    ToggleButton(_btnHybridDecrypt);
    // temporary like this since hybrid decryption does not have synchronous method
    await PerformWithProgress(async () =>
      await PerformHybridDecryption(rsa, aes));
    ToggleButton(_btnHybridDecrypt);
  }

  private async Task PerformWithProgress(Func<Task> runCryptographicOperation)
  {
    ToggleProgress(true);

    try
    {
      var selectedDataFormatCache = _selectedDataFormat;
      await runCryptographicOperation();
      ToggleProgress(false);

      if (selectedDataFormatCache != DataFormat.File) return;

      if (runCryptographicOperation.Method.Name.Contains(
        "Encrypt", StringComparison.OrdinalIgnoreCase))
      {
        MessageNotifier.ShowSuccess(
          $"Your encrypted files has been saved to {ENCRYPTED_OUTPUT_DIRECTORY} folder");
        return;
      }

      MessageNotifier.ShowSuccess(
        $"Your decrypted files has been saved to {DECRYPTION_OUTPUT_DIRECTORY} folder");
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

  private Task PerformHybridEncryption(RSA rsa, Aes aes)
  {
    var (rsaKey, aesKey) = LoadEncryptionKeys();

    var hybridEncryption = CryptoComponentFactory.CreateHybridEncryption(
      rsa, aes, _selectedRSAPadding, _selectedAESPadding, _selectedHashAlgorithm
    );

    return HybridEncryptData(hybridEncryption, rsaKey, aesKey);
  }

  private Task PerformHybridDecryption(RSA rsa, Aes aes)
  {
    var rsaKey = LoadRSAKey();

    var hybridEncryption = CryptoComponentFactory.CreateHybridEncryption(
      rsa, aes, _selectedRSAPadding, _selectedAESPadding, _selectedHashAlgorithm
    );

    return HybridDecryptData(hybridEncryption, rsaKey);
  }

  private async Task HybridEncryptData(
    HybridEncryption hybridEncryption, RSAKey rsaKey, AESKey aesKey)
      => await hybridEncryption.EncryptFileAsync(
        inputFilePath: _txtDataOrFilePath.Text,
        outputFilePath: GetEncryptedFilePath(_txtDataOrFilePath.Text),
        rsaKey,
        aesKey);

  private async Task HybridDecryptData(
    HybridEncryption hybridEncryption, RSAKey rsaKey)
      => await hybridEncryption.DecryptFileAsync(
      inputFilePath: _txtDataOrFilePath.Text,
      outputFilePath: GetDecryptedFilePath(_txtDataOrFilePath.Text),
      rsaKey);


  private Task PerformRSAEncryptionAsync(RSA rsa)
  {
    var publicKeyPem = _keyStorage.ReadSingleRSAKey(_importedPublicKeyFilePath);
    var rsaEncryption = CryptoComponentFactory.CreateRSAEncryption(
      rsa, _selectedRSAPadding);

    if (_cbUseMultithreading.Checked)
      return Task.Run(() => EncryptData(rsaEncryption, publicKeyPem));

    EncryptData(rsaEncryption, publicKeyPem);
    return Task.CompletedTask;
  }

  private Task PerformRSADecryptionAsync(RSA rsa)
  {
    var privateKeyPem = _keyStorage.ReadSingleRSAKey(_importedPrivateKeyFilePath);
    var rsaEncryption = CryptoComponentFactory.CreateRSAEncryption(
      rsa, _selectedRSAPadding);

    if (_cbUseMultithreading.Checked)
      return Task.Run(() => DecryptData(rsaEncryption, privateKeyPem));

    DecryptData(rsaEncryption, privateKeyPem);
    return Task.CompletedTask;
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

  private (RSAKey rsaKey, AESKey aesKey) LoadEncryptionKeys()
  {
    var rsaKey = LoadRSAKey();
    var aesKey = _keyStorage.ReadAESKey(_importedAESKeyFilePath);

    return (rsaKey, aesKey);
  }

  private RSAKey LoadRSAKey()
  {
    var publicKey = _keyStorage.ReadSingleRSAKey(_importedPublicKeyFilePath);
    var privateKey = _keyStorage.ReadSingleRSAKey(_importedPrivateKeyFilePath);

    return new RSAKey(publicKey, privateKey);
  }

  private static string GetEncryptedFilePath(string inputPath)
  {
    var encryptedDirectory = Path.Combine(
      Path.GetDirectoryName(Application.ExecutablePath)!, ENCRYPTED_OUTPUT_DIRECTORY);
    Directory.CreateDirectory(encryptedDirectory);
    return Path.Combine(encryptedDirectory,
      $"{Path.GetFileNameWithoutExtension(inputPath)}-encrypted{Path.GetExtension(inputPath)}");
  }

  private static string GetDecryptedFilePath(string inputPath)
  {
    var decryptedDirectory = Path.Combine(
      Path.GetDirectoryName(Application.ExecutablePath)!, DECRYPTION_OUTPUT_DIRECTORY);
    Directory.CreateDirectory(decryptedDirectory);
    return Path.Combine(decryptedDirectory,
      $"{Path.GetFileNameWithoutExtension(inputPath)}-decrypted{Path.GetExtension(inputPath)}");
  }

  private bool AreInputsForRSAValid()
    => !string.IsNullOrEmpty(_txtDataOrFilePath.Text) &&
    !string.IsNullOrEmpty(_txtImportedPublicKeyName.Text) &&
    !string.IsNullOrEmpty(_txtImportedPrivateKeyName.Text);

  private bool AreInputsForHybridValid()
    => AreInputsForRSAValid() &&
    !string.IsNullOrEmpty(_txtImportedAESKeyName.Text);

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

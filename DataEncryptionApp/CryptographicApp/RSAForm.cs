using CryptographicApp.CryptographicCores.Asymmetric;
using CryptographicApp.CryptographicCores.HashGenerators;
using CryptographicApp.CryptographicCores.Hybrid;
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
    _btnRSAEncrypt.Click += async (s, e) => await OnEncryptClickedAsync();
    _btnRSADecrypt.Click += async (s, e) => await OnDecryptClickedAsync();
    _btnBrowse.Click += (s, e) => OnBrowseClicked();

    _cbAESPadding.SelectedValueChanged += (s, e) => OnSelectedAESPaddingChanged();
    _cbRSAKeySize.SelectedValueChanged += (s, e) => OnSelectedAESKeySizeChanged();
    _cbHashAlgorithm.SelectedValueChanged += (s, e) => OnSelectedHashAlgorithmChanged();

    _btnGenerateAESKey.Click += (s, e) => OnProcessStart();
    _btnHybridEncrypt.Click += (s, e) => OnProcessStart();

    _btnGenerateAESKey.Click += async (s, e) => await OnGenerateAESKeyClickedAsync();
    _btnImportAESKey.Click += (s, e) => OnImportAESKeyClicked();
    _btnHybridEncrypt.Click += async (s, e) => await OnHybridEncryptClickedAsync();
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

  private async Task OnEncryptClickedAsync()
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

  private async Task OnDecryptClickedAsync()
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
    if (!AreInputsForHybridValid())
    {
      MessageNotifier.ShowError(
        "Data or file path and imported key cannot be empty.");
      return;
    }

    using var rsa = RSA.Create();
    using var aes = Aes.Create();

    ToggleButton(_btnHybridEncrypt);
    await PerformWithProgress(() => PerformHybridEncryptionAsync(rsa, aes));
    ToggleButton(_btnHybridEncrypt);
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

  private Task PerformHybridEncryptionAsync(RSA rsa, Aes aes)
  {
    var publicKeyPem = _keyStorage.ReadSingleRSAKey(_importedPublicKeyFilePath);
    var privateKeyPem = _keyStorage.ReadSingleRSAKey(_importedPrivateKeyFilePath);

    var rsaKey = new RSAKey(publicKeyPem, privateKeyPem);
    var aesKey = _keyStorage.ReadAESKey(_importedAESKeyFilePath);

    var rsaEncryption = new RSAEncryption(rsa, _selectedRSAPadding);

    var hashGenerator = DetermineHashGenerator(_selectedHashAlgorithm);
    var metadataHeader = new HeaderMetadataHandler(hashGenerator, rsaEncryption);

    var aesEncryption = new AESEncryption(aes);
    aesEncryption.SetPadding(_selectedAESPadding);

    var hybridEncryption = new HybridEncryption(
      rsaEncryption, aesEncryption, metadataHeader
    );

    return _cbUseMultithreading.Checked
      ? Task.Run(() => HybridEncryptData(hybridEncryption, rsaKey, aesKey))
      : Task.CompletedTask.ContinueWith(_ =>
          HybridEncryptData(hybridEncryption, rsaKey, aesKey));
  }

  private async Task HybridEncryptData(
    HybridEncryption hybridEncryption, RSAKey rsaKey, AESKey aesKey)
      => await hybridEncryption.EncryptFileAsync(
        inputFilePath: _txtDataOrFilePath.Text,
        outputFilePath: GetEncryptedFilePath(_txtDataOrFilePath.Text),
        rsaKey,
        aesKey);

  private static IHashGenerator DetermineHashGenerator(Enums.HashAlgorithm hashAlgorithm)
  {
    if (hashAlgorithm == Enums.HashAlgorithm.MD5) return new MD5HashGenerator();
    if (hashAlgorithm == Enums.HashAlgorithm.SHA1) return new SHA1HashGenerator();
    if (hashAlgorithm == Enums.HashAlgorithm.SHA3_256) return new SHA3_512HashGenerator();

    return new SHA256HashGenerator();
  }

  private Task PerformRSAEncryptionAsync(RSA rsa)
  {
    var publicKeyPem = _keyStorage.ReadSingleRSAKey(_importedPublicKeyFilePath);
    var rsaEncryption = new RSAEncryption(rsa, _selectedRSAPadding);

    return _cbUseMultithreading.Checked
      ? Task.Run(() => EncryptData(rsaEncryption, publicKeyPem))
      : Task.CompletedTask.ContinueWith(_ => EncryptData(rsaEncryption, publicKeyPem));
  }

  private Task PerformRSADecryptionAsync(RSA rsa)
  {
    var privateKeyPem = _keyStorage.ReadSingleRSAKey(_importedPrivateKeyFilePath);
    var rsaEncryption = new RSAEncryption(rsa, _selectedRSAPadding);

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

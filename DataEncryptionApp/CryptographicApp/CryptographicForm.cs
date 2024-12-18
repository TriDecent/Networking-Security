using System.Security.Cryptography;

namespace CryptographicApp;

public partial class CryptographicForm : Form
{
  private readonly Button _btnBrowse, _btnGenerateKey, _btnImportKey;
  private readonly Button _btnEncrypt, _btnDecrypt;
  private readonly ComboBox _cbDataFormat, _cbPadding;
  private readonly TextBox _txtDataOrFilePath, _txtResult;

  public CryptographicForm()
  {
    InitializeComponent();

    _btnBrowse = btnBrowse;
    _btnGenerateKey = btnGenerateKey;
    _btnImportKey = btnImportKey;
    _btnEncrypt = btnEncrypt;
    _cbDataFormat = cbDataFormat;
    _cbPadding = cbPadding;
    _txtDataOrFilePath = txtDataOrFilePath;
    _txtResult = txtResult;

    _cbDataFormat.DataSource = Enum.GetValues<DataFormat>();
    _cbPadding.DataSource = new[]
    {
      RSAEncryptionPadding.Pkcs1, RSAEncryptionPadding.OaepSHA1,
      RSAEncryptionPadding.OaepSHA256, RSAEncryptionPadding.OaepSHA384,
      RSAEncryptionPadding.OaepSHA512, RSAEncryptionPadding.OaepSHA3_256,
      RSAEncryptionPadding.OaepSHA3_384, RSAEncryptionPadding.OaepSHA3_512
    };
  }
}

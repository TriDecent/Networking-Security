using HashCalculator.Enums;
using HashCalculator.HashGenerators;

namespace HashCalculator;

public partial class HashCalculatorForm : Form
{
  private readonly ComboBox _cbDataFormat;

  private readonly Label _lblData;

  private readonly TextBox _txtData;
  private readonly TextBox _txtMD5Hash;
  private readonly TextBox _txtSHA1Hash;
  private readonly TextBox _txtSHA2Hash;
  private readonly TextBox _txtSHA3_512Hash;

  private readonly CheckBox _cbUseMD5;
  private readonly CheckBox _cbUseSHA1;
  private readonly CheckBox _cbUseSHA256;
  private readonly CheckBox _cbUseSHA3_512;

  private DataFormat _dataFormat;

  public HashCalculatorForm()
  {
    InitializeComponent();

    _txtData = txtData;
    _lblData = lblData;
    _txtMD5Hash = txtMD5Hash;
    _txtSHA1Hash = txtSHA1Hash;
    _txtSHA2Hash = txtSHA256Hash;
    _txtSHA3_512Hash = txtSHA3_512Hash;
    _cbUseMD5 = cbMD5;
    _cbUseSHA1 = cbSHA1;
    _cbUseSHA256 = cbSHA256;
    _cbUseSHA3_512 = cbSHA3_512;

    _cbDataFormat = cbDataFormat;

    _cbDataFormat.DataSource = Enum.GetValues(typeof(DataFormat));

    _cbDataFormat.SelectedValueChanged += OnDataFormatChanged;

    _txtData.TextChanged += OnDataChanged;
    _cbUseMD5.CheckedChanged += OnDataChanged;
    _cbUseSHA1.CheckedChanged += OnDataChanged;
    _cbUseSHA256.CheckedChanged += OnDataChanged;
    _cbUseSHA3_512.CheckedChanged += OnDataChanged;

    _cbUseMD5.CheckedChanged += OnCheckedChanged;
    _cbUseSHA1.CheckedChanged += OnCheckedChanged;
    _cbUseSHA256.CheckedChanged += OnCheckedChanged;
    _cbUseSHA3_512.CheckedChanged += OnCheckedChanged;
  }

  private void OnDataFormatChanged(object? sender, EventArgs e)
  {
    _dataFormat = (DataFormat)_cbDataFormat.SelectedItem!;

    if (_dataFormat == DataFormat.File)
    {
      _lblData.Text = "File Path";
      _txtData.ReadOnly = true;

      using var openFileDialog = new OpenFileDialog();
      if (openFileDialog.ShowDialog() == DialogResult.OK)
      {
        _txtData.Text = openFileDialog.FileName;
      }

      return;
    }

    _lblData.Text = "Data";
    _txtData.ReadOnly = false;
    _txtData.Text = string.Empty;
    ClearHashFields();
  }

  private void OnDataChanged(object? sender, EventArgs e)
  {
    if (string.IsNullOrWhiteSpace(_txtData.Text))
    {
      ClearHashFields();
      return;
    }

    try
    {
      if (_cbUseMD5.Checked) _txtMD5Hash.Text = GenerateHash(new MD5HashGenerator());
      if (_cbUseSHA1.Checked) _txtSHA1Hash.Text = GenerateHash(new SHA1HashGenerator());
      if (_cbUseSHA256.Checked) _txtSHA2Hash.Text = GenerateHash(new SHA256HashGenerator());
      if (_cbUseSHA3_512.Checked) _txtSHA3_512Hash.Text = GenerateHash(new SHA3_512HashGenerator());
    }
    catch (Exception ex) when (ex is InvalidOperationException || ex is FormatException)
    {
      MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }

  private void ClearHashFields()
  {
    _txtMD5Hash.Text = string.Empty;
    _txtSHA1Hash.Text = string.Empty;
    _txtSHA2Hash.Text = string.Empty;
    _txtSHA3_512Hash.Text = string.Empty;
  }

  private string GenerateHash(IHashGenerator hashGenerator)
    => hashGenerator.GenerateHash(_txtData.Text, _dataFormat);

  private void OnCheckedChanged(object? sender, EventArgs e)
  {
    _txtMD5Hash.Text = _cbUseMD5.Checked ? _txtMD5Hash.Text : string.Empty;
    _txtSHA1Hash.Text = _cbUseSHA1.Checked ? _txtSHA1Hash.Text : string.Empty;
    _txtSHA2Hash.Text = _cbUseSHA256.Checked ? _txtSHA2Hash.Text : string.Empty;
    _txtSHA3_512Hash.Text = _cbUseSHA3_512.Checked ? _txtSHA3_512Hash.Text : string.Empty;
  }
}
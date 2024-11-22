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

  private readonly CheckBox _cbUseMD5;
  private readonly CheckBox _cbUseSHA1;
  private readonly CheckBox _cbUseSHA256;

  private DataFormat _dataFormat;

  public HashCalculatorForm()
  {
    InitializeComponent();

    _txtData = txtData;
    _lblData = lblData;
    _txtMD5Hash = txtMD5Hash;
    _txtSHA1Hash = txtSHA1Hash;
    _txtSHA2Hash = txtSHA256Hash;
    _cbUseMD5 = cbMD5;
    _cbUseSHA1 = cbSHA1;
    _cbUseSHA256 = cbSHA256;
    _cbDataFormat = cbDataFormat;

    _cbDataFormat.DataSource = Enum.GetValues(typeof(DataFormat));

    _cbDataFormat.SelectedValueChanged += OnDataFormatChanged;

    _txtData.TextChanged += OnDataChanged;
    _cbUseMD5.CheckedChanged += OnDataChanged;
    _cbUseSHA1.CheckedChanged += OnDataChanged;
    _cbUseSHA256.CheckedChanged += OnDataChanged;
  }

  private void OnDataFormatChanged(object? sender, EventArgs e)
  {
    _dataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), _cbDataFormat.SelectedItem?.ToString()!);

    if (_dataFormat == DataFormat.File)
    {
      _lblData.Text = "File Path";
      _txtData.ReadOnly = true;

      using var openFileDialog = new OpenFileDialog();
      if (openFileDialog.ShowDialog() == DialogResult.OK)
      {
        _txtData.Text = openFileDialog.FileName;
      }
    }
  }

  private void OnDataChanged(object? sender, EventArgs e)
  {
    var useMD5 = _cbUseMD5.Checked;
    var useSHA1 = _cbUseSHA1.Checked;
    var useSHA2 = _cbUseSHA256.Checked;

    if (useMD5)
    {
      var _hashGenerator = new MD5HashGenerator();
      try { _txtMD5Hash.Text = _hashGenerator.GenerateHash(_txtData.Text, _dataFormat); }
      catch (InvalidOperationException ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      catch (FormatException ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    if (useSHA1)
    {
      var _hashGenerator = new SHA1HashGenerator();
      _txtSHA1Hash.Text = _hashGenerator.GenerateHash(_txtData.Text, _dataFormat);
    }

    if (useSHA2)
    {
      var _hashGenerator = new SHA256HashGenerator();
      _txtSHA2Hash.Text = _hashGenerator.GenerateHash(_txtData.Text, _dataFormat);
    }
  }
}

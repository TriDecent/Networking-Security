namespace HashCalculator;

public partial class HashCalculatorForm : Form
{
  private readonly ComboBox _cbDataFormat;
  private readonly TextBox _txtData;
  private readonly TextBox _txtMD5Hash;
  private readonly TextBox _txtSHA1Hash;
  private readonly TextBox _txtSHA2Hash;

  private readonly Label _lblData;

  private DataFormat _dataFormat;

  public HashCalculatorForm()
  {
    InitializeComponent();

    _cbDataFormat = cbDataFormat;
    _txtData = txtData;
    _txtMD5Hash = txtMD5Hash;
    _txtSHA1Hash = txtSHA1Hash;
    _txtSHA2Hash = txtSHA2Hash;
    _lblData = lblData;

    _cbDataFormat.DataSource = Enum.GetValues(typeof(DataFormat));

    _cbDataFormat.SelectedValueChanged += OnDataFormatChanged;
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
}
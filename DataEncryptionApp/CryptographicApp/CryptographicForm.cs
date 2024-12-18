namespace CryptographicApp;

public partial class CryptographicForm : Form
{
  private Form _activeForm;

  public CryptographicForm()
  {
    InitializeComponent();

    IsMdiContainer = true; 

    var toolStrip = new ToolStrip();

    var btnRSA = new ToolStripButton("RSA");
    var btnPlayFair = new ToolStripButton("PlayFair");

    toolStrip.Items.AddRange([btnRSA, btnPlayFair]);

    OpenForm(new RSAForm());

    btnRSA.Click += (s, e) => OpenForm(new RSAForm());
    btnPlayFair.Click += (s,e) => OpenForm(new PlayFairForm());

    Controls.Add(toolStrip);
  }

  private void OpenForm(Form form)
  {
    _activeForm?.Close();

    _activeForm = form;
    form.MdiParent = this;
    form.FormBorderStyle = FormBorderStyle.Fixed3D;
    form.Dock = DockStyle.None;
    form.Show();
  }
}

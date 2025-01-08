using CryptographicApp.CryptographicCores.Symmetric;

namespace CryptographicApp;

public partial class PlayFairForm : Form
{
    readonly PlayFair5x5 _playFair5X5;
    readonly PlayFair6x6 _playFair6X6;
    private bool _is5x5 = false;

    public PlayFairForm()
    {
        InitializeComponent();
        SetupInit();
        _playFair5X5 = new PlayFair5x5();
        _playFair6X6 = new PlayFair6x6();
    }

    private void ButtonGenerateMatrix_Click(object sender, EventArgs e)
    {
        if (_is5x5)
        {
            string key = textBoxKey.Text;
            _playFair5X5.CreateMatrix5x5(tableLayoutPanel5x5, key);
        }
        else
        {
            string key = textBoxKey.Text;
            _playFair6X6.CreateMatrix6x6(tableLayoutPanel6x6, key);
        }

    }

    private void ButtonEncrypt_Click(object sender, EventArgs e)
    {
        if (_is5x5)
        {
            string plainText = textBoxPlainText.Text;
            if (string.IsNullOrEmpty(plainText))
            {
                MessageBox.Show("Please enter plaintext", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string result = _playFair5X5.Encrypt(plainText);
            textBoxResult.Text = result;
        }
        else
        {
            string plainText = textBoxPlainText.Text;
            if (string.IsNullOrEmpty(plainText))
            {
                MessageBox.Show("Please enter plaintext", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string result = _playFair6X6.Encrypt(plainText);
            textBoxResult.Text = result;
        }

    }

    private void SetupInit()
    {
        tableLayoutPanel6x6.Visible = false;
        tableLayoutPanel5x5.Visible = true;
        radioButton5x5.Checked = true;
        radioButton6x6.Checked = false;
    }

    private void ButtonDecrypt_Click(object sender, EventArgs e)
    {
        if (_is5x5)
        {
            string cipherText = textBoxCipherText.Text;
            if (string.IsNullOrEmpty(cipherText))
            {
                MessageBox.Show("Please enter cipher text", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string result = _playFair5X5.Decrypt(cipherText);
            textBoxResult.Text = result;
        }
        else
        {
            string cipherText = textBoxCipherText.Text;
            if (string.IsNullOrEmpty(cipherText))
            {
                MessageBox.Show("Please enter cipher text", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string result = _playFair6X6.Decrypt(cipherText);
            textBoxResult.Text = result;
        }

    }

    private void RadioButton5x5_CheckedChanged(object sender, EventArgs e)
    {
        if (radioButton5x5.Checked)
        {
            tableLayoutPanel5x5.Visible = true;
            tableLayoutPanel6x6.Visible = false;
            _is5x5 = true;
        }
        else
        {
            tableLayoutPanel5x5.Visible = false;
            tableLayoutPanel6x6.Visible = true;
            _is5x5 = false;
        }
    }

    private void RadioButton6x6_CheckedChanged(object sender, EventArgs e)
    {
        if (radioButton6x6.Checked)
        {
            tableLayoutPanel6x6.Visible = true;
            tableLayoutPanel5x5.Visible = false;
            _is5x5 = false;
            ResetContent();
        }
        else
        {
            tableLayoutPanel6x6.Visible = false;
            tableLayoutPanel5x5.Visible = true;
            _is5x5 = true;
            ResetContent();
        }
    }

    private void ResetContent()
    {
        textBoxPlainText.Text = "";
        textBoxCipherText.Text = "";
        textBoxKey.Text = "";
        textBoxResult.Text = "";
        tableLayoutPanel5x5.Controls.Clear();
        tableLayoutPanel6x6.Controls.Clear();
        _playFair5X5.ResetMatrixKey();
        _playFair6X6.ResetMatrixKey();
    }
}

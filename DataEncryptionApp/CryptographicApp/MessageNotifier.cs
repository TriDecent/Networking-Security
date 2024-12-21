namespace CryptographicApp;

public static class MessageNotifier
{
  public static void ShowWarning(string message) => MessageBox.Show(
    message,
    "Warning",
    MessageBoxButtons.OK,
    MessageBoxIcon.Warning);

  public static void ShowSuccess(string message) => MessageBox.Show(
    message,
    "Success",
    MessageBoxButtons.OK,
    MessageBoxIcon.Information);

  public static void ShowError(string message) => MessageBox.Show(
    message,
    "Error",
    MessageBoxButtons.OK,
    MessageBoxIcon.Error);
}
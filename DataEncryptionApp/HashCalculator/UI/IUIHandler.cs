namespace HashCalculator;

public interface IUIHandler
{
  void Show(string message);
}

public class HashCalculatorUIHandler : IUIHandler
{
  private readonly TextBox _textBox;

  public HashCalculatorUIHandler(TextBox textBox) => _textBox = textBox;

  public void Show(string message) => _textBox.Text = message;
}
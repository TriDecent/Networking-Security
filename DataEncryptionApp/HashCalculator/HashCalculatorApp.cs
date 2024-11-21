namespace HashCalculator;

internal class HashCalculatorApp
{
  private readonly IUIHandler _uiHandler;
  private readonly IHashGenerator _hashGenerator;

  public HashCalculatorApp(IUIHandler uiHandler, IHashGenerator hashGenerator)
  {
    _uiHandler = uiHandler;
    _hashGenerator = hashGenerator;
  }
}

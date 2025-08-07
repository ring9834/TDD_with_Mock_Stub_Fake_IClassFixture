namespace CalculatorApp;

public class Calculator
{
    private readonly ILogger _logger;

    public Calculator(ILogger logger)
    {
        _logger = logger;
    }

    public int Add(int a, int b)
    {
        _logger.Log($"Adding {a} and {b}");
        return a + b;
    }

    public int Divide(int a, int b)
    {
        if (b == 0)
        {
            _logger.Log("Attempted division by zero");
            throw new DivideByZeroException();
        }
        _logger.Log($"Dividing {a} by {b}");
        return a / b;
    }

}

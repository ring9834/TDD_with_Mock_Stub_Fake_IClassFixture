namespace CalculatorApp;

public class Calculator_use_Fixture
{
    private readonly ILogger _logger;
    private readonly IRepository _repository;

    public Calculator_use_Fixture(ILogger logger, IRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public int Add(int a, int b)
    {
        _logger.Log($"Adding {a} and {b}");
        int result = a + b;
        _repository.SaveResult(result);
        return result;
    }

    public int Divide(int a, int b)
    {
        if (b == 0)
        {
            _logger.Log("Attempted division by zero");
            throw new DivideByZeroException();
        }
        _logger.Log($"Dividing {a} by {b}");
        int result = a / b;
        _repository.SaveResult(result);
        return result;
    }
}

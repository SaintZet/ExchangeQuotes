using ExchangeQuotes.Math.Statistic;

namespace ExchangeQuotes.Math.UnitTests.Statistic;

public class ModeCalculatorTest
{
    private const int _roundDigits = 9;

    [Theory]
    [InlineData(new int[] { 1, 2, 3, 2, 2, 2 }, 2)]
    [InlineData(new int[] { 2, 3, 3, 5, 7, 10 }, 3)]
    [InlineData(new int[] { 10, 2, 38, 23, 38, 23, 21 }, 23)]
    public void GetCurrentResult_SimpleWork_Mode(int[] integers, double expected)
    {
        // Arrange
        ModeCalculator calculator = new();

        // Act
        foreach (var integer in integers)
        {
            calculator.AddNumberToSequence(integer);
        }

        double result = calculator.GetCurrentResult();

        // Assert
        Assert.Equal(System.Math.Round(expected, _roundDigits), System.Math.Round(result, _roundDigits));
    }

    [Theory]
    [InlineData(new int[] { 1, 2, 3, 2, 2, 2 }, 2)]
    [InlineData(new int[] { 2, 3, 3, 5, 7, 10 }, 3)]
    [InlineData(new int[] { 10, 2, 38, 23, 38, 23, 21 }, 38)]
    public void GetCurrentResult_WorkInThreads_Mode(int[] integers, double expected)
    {
        // Arrange
        ModeCalculator calculator = new();
        AutoResetEvent signal = new(false);
        int numbersForSignalWakeUp = 5;

        Task task1 = DoWork(calculator, integers, numbersForSignalWakeUp, signal);
        Task<double> task2 = WakeUpAndTakeResult(calculator, signal);

        // Act
        Task.WaitAll(task1, task2);

        // Assert
        Assert.Equal(System.Math.Round(expected, _roundDigits), System.Math.Round(task2.Result, _roundDigits));
    }

    [Theory]
    [InlineData(new int[] { 1, 2, 3 }, new int[] { 2, 2, 2 }, 2)]
    [InlineData(new int[] { 2, 3, 3 }, new int[] { 5, 7, 10 }, 3)]
    [InlineData(new int[] { 10, 2, 38 }, new int[] { 23, 38, 23, 21 }, 23)]
    public void GetCurrentResult_AsyncFilling_Mode(int[] firstThreadintegers, int[] secondThreadintegers, double expected)
    {
        // Arrange
        ModeCalculator calculator = new();
        Task task1 = DoWork(calculator, firstThreadintegers);
        Task task2 = DoWork(calculator, secondThreadintegers);

        // Act
        Task.WaitAll(task1, task2);
        double result = calculator.GetCurrentResult();

        // Assert
        Assert.Equal(System.Math.Round(expected, _roundDigits), System.Math.Round(result, _roundDigits));
    }

    private Task DoWork(ModeCalculator calculator, int[] integers)
    {
        return Task.Run(() =>
        {
            foreach (var integer in integers)
            {
                calculator.AddNumberToSequence(integer);
            }

            return Task.CompletedTask;
        });
    }

    private Task DoWork(ModeCalculator calculator, int[] integers, int indexForSignal, AutoResetEvent signal)
    {
        return Task.Run(() =>
        {
            int i = 0;

            foreach (var integer in integers)
            {
                calculator.AddNumberToSequence(integer);

                i++;

                if (i == indexForSignal)
                {
                    signal.Set();
                    Thread.Sleep(100);
                }
            }

            return Task.CompletedTask;
        });
    }

    private Task<double> WakeUpAndTakeResult(ModeCalculator calculator, AutoResetEvent signal)
    {
        return Task.Run(() =>
        {
            signal.WaitOne();

            return calculator.GetCurrentResult();
        });
    }
}
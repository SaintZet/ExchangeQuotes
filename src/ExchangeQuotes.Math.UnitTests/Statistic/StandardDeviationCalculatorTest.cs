using ExchangeQuotes.Math.Statistic;

namespace ExchangeQuotes.Math.UnitTests.Statistic;

public class StandardDeviationCalculatorTest
{
    private const int _roundDigits = 9;

    [Theory]
    [InlineData(new int[] { 1, 2, 3 }, 1)]
    [InlineData(new int[] { 2, 3, 3, 5, 7, 10 }, 3.0331501776206)]
    [InlineData(new int[] { 10, 2, 38, 23, 38, 23, 21 }, 13.284434142115)]
    public void GetCurrentResult_SimpleWork_StandardDeviation(int[] integers, double expected)
    {
        // Arrange
        StandardDeviationCalculator calculator = new();

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
    [InlineData(new int[] { 1, 2, 3, 2, 2, 2 }, 0.70710678118655)]
    [InlineData(new int[] { 2, 3, 3, 5, 7, 10 }, 2)]
    [InlineData(new int[] { 10, 2, 38, 23, 38, 23, 21 }, 16.254230218623)]
    public void GetCurrentResult_WorkInThreads_StandardDeviation(int[] integers, double expected)
    {
        // Arrange
        StandardDeviationCalculator calculator = new();
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
    [InlineData(new int[] { 1, 2, 3 }, new int[] { 2, 2, 2 }, 0.63245553203368)]
    [InlineData(new int[] { 2, 3, 3 }, new int[] { 5, 7, 10 }, 3.0331501776206)]
    [InlineData(new int[] { 10, 2, 38 }, new int[] { 23, 38, 23, 21 }, 13.284434142115)]
    public void GetCurrentResult_AsyncFilling_StandardDeviation(int[] firstThreadintegers, int[] secondThreadintegers, double expected)
    {
        // Arrange
        StandardDeviationCalculator calculator = new();
        Task task1 = DoWork(calculator, firstThreadintegers);
        Task task2 = DoWork(calculator, secondThreadintegers);

        // Act
        Task.WaitAll(task1, task2);
        double result = calculator.GetCurrentResult();

        // Assert
        Assert.Equal(System.Math.Round(expected, _roundDigits), System.Math.Round(result, _roundDigits));
    }

    private Task DoWork(StandardDeviationCalculator calculator, int[] integers)
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

    private Task DoWork(StandardDeviationCalculator calculator, int[] integers, int indexForSignal, AutoResetEvent signal)
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

    private Task<double> WakeUpAndTakeResult(StandardDeviationCalculator calculator, AutoResetEvent signal)
    {
        return Task.Run(() =>
        {
            signal.WaitOne();

            return calculator.GetCurrentResult();
        });
    }
}
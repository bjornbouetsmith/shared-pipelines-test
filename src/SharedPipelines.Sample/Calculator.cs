namespace SharedPipelines.Sample;

/// <summary>
/// Minimal calculator used to give the shared pipelines something to build,
/// test and pack.
/// </summary>
public static class Calculator
{
    /// <summary>Adds two integers, failing on overflow instead of wrapping.</summary>
    public static int Add(int left, int right) => checked(left + right);

    /// <summary>Integer division that rejects a zero divisor explicitly.</summary>
    /// <exception cref="DivideByZeroException">The divisor is zero.</exception>
    public static int Divide(int dividend, int divisor)
    {
        if (divisor == 0)
        {
            throw new DivideByZeroException("The divisor must not be zero.");
        }

        return dividend / divisor;
    }
}

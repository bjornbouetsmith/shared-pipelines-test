using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharedPipelines.Sample.Tests;

[TestClass]
public sealed class CalculatorTest
{
    [TestMethod]
    public void Add_ForTwoPositiveValues_ReturnsSum()
    {
        var result = Calculator.Add(2, 3);

        Assert.AreEqual(5, result);
    }

    [TestMethod]
    public void Add_ForNegativeAndPositiveValue_ReturnsDifference()
    {
        var result = Calculator.Add(-10, 4);

        Assert.AreEqual(-6, result);
    }

    [TestMethod]
    [DataRow(10, 2, 5)]
    [DataRow(-9, 3, -3)]
    [DataRow(7, 2, 3)]
    public void Divide_ForNonZeroDivisor_ReturnsQuotient(int dividend, int divisor, int expected)
    {
        var result = Calculator.Divide(dividend, divisor);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Divide_ForZeroDivisor_ThrowsDivideByZeroException()
    {
        var exception = Assert.ThrowsExactly<DivideByZeroException>(() => Calculator.Divide(1, 0));

        Assert.AreEqual("The divisor must not be zero.", exception.Message);
    }
}

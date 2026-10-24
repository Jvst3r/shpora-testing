
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true)]
    [TestCase(5, 6, false)]
    [TestCase(0, 0, false)]
    public void NumberValidatorConstructor_ThrowsException_WhenInvalidData(int presicion,
                                                                           int scale,
                                                                           bool onlyPositive = false)
    {
        var act = () => new NumberValidator(presicion, scale, onlyPositive);

        act.Should().Throw<ArgumentException>($"При параметрах presicion = {presicion} и scale = {scale}");

    }

    [TestCase("0", 17, 2, true)]
    [TestCase("0.0", 17, 2, true)]
    [TestCase("+1.23", 4, 2, true)]
    [TestCase("12.34", 5, 2, false)]
    [TestCase("12,34", 5, 2, false)]
    public void IsValidNumber_ReturnTrue_WhenValidData(string input,int precision, int scale,
                                                       bool onlyPositive)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        var act = validator.IsValidNumber(input);

        act.Should().BeTrue($"значение '{input}' должно быть валидным при precision={precision}, scale={scale}");
    }

    [TestCase("00.00", 3, 2, true)]
    [TestCase("+0.00", 3, 2, true)]
    [TestCase("0.000", 17, 2, true)]
    [TestCase("-1.23", 3, 2, true)]
    [TestCase("-0.00", 3, 2, true)]
    [TestCase("a.sd", 3, 2, true)]
    [TestCase("", 3, 2, true)]
    [TestCase(null, 3, 2, true)]
    [TestCase("1.2.3", 5, 2, false)]
    public void IsValidNumber_ReturnFalse_WhenInvalidData(string input, int precision, int scale,
                                                          bool onlyPositive)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        var act = validator.IsValidNumber(input);

        act.Should().BeFalse($"значение '{input}' должно быть НЕвалидным при precision={precision}, scale={scale}");
    }
}
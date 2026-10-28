using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, TestName = "NumberValidatorConstructor_ThrowsException_WhenNegativePrecision")]
    [TestCase(5, 6, false, TestName = "NumberValidatorConstructor_ThrowsException_WhenScaleGreaterThanPrecision")]
    [TestCase(0, 0, false, TestName = "NumberValidatorConstructor_ThrowsException_WhenZeroPrecision")]
    [TestCase(5, -1, false, TestName = "NumberValidatorConstructor_ThrowsException_WhenNegativeScale")]
    public void NumberValidatorConstructor_ThrowsException_WhenInvalidData(int presicion,
                                                                           int scale,
                                                                           bool onlyPositive)
    {
        var act = () => new NumberValidator(presicion, scale, onlyPositive);

        act.Should().Throw<ArgumentException>($"При параметрах presicion = {presicion} и scale = {scale}");

    }

    [TestCase("0", 17, 2, true, TestName = "IsValidNumber_ReturnTrue_WhenZero")]
    [TestCase("0.0", 17, 2, true, TestName = "IsValidNumber_ReturnTrue_WhenZeroWithDecimal")]
    [TestCase("+1.23", 4, 2, true, TestName = "IsValidNumber_ReturnTrue_WhenPositive")]
    [TestCase("-1.23", 4, 2, false, TestName = "IsValidNumber_ReturnTrue_WhenNegative")]
    [TestCase("12.34", 5, 2, false, TestName = "IsValidNumber_ReturnTrue_WhenFitsExactlyPresicion")]
    [TestCase("12,34", 5, 2, false, TestName = "IsValidNumber_ReturnTrue_WhenCommaAsSeparator")]
    public void IsValidNumber_ReturnTrue_WhenValidData(string input,int precision, int scale,
                                                       bool onlyPositive)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        var act = validator.IsValidNumber(input);

        act.Should().BeTrue($"значение '{input}' должно быть валидным при precision={precision}, scale={scale}");
    }

    [TestCase("00.00", 3, 2, true, TestName = "IsValidNumber_ReturnFalse_ExceedsPresicion")]
    [TestCase("+0.00", 3, 2, true, TestName = "IsValidNumber_ReturnsFalse_WhenSignExceedsPrecision")]
    [TestCase("0.000", 17, 2, true, TestName = "IsValidNumber_ReturnsFalse_WhenExceedsScale")]
    [TestCase("-1.23", 3, 2, true, TestName = "IsValidNumber_ReturnsFalse_WhenNegativeWhenOnlyPositive")]
    [TestCase("a.sd", 3, 2, true, TestName = "IsValidNumber_ReturnFalse_WhenLettersInNumber")]
    [TestCase("", 3, 2, true, TestName = "IsValidNumber_ReturnFalse_WhenNumberIsEmpty")]
    [TestCase(null, 3, 2, true, TestName = "IsValidNumber_ReturnFalse_WhenNoNumber")]
    [TestCase("1.2.3", 5, 2, false, TestName = "IsValidNumber_ReturnFalse_WhenMoreThenTwoPiecesOfNumber")]
    [TestCase("1;23", 5, 2, false, TestName = "IsValidNumber_ReturnFalse_WhenInvalidSeparator")]
    [TestCase("1 23", 5, 2, false, TestName ="IsValidNumber_ReturnFalse_WhenNoSeparator")]
    public void IsValidNumber_ReturnFalse_WhenInvalidData(string input, int precision, int scale,
                                                          bool onlyPositive)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        var act = validator.IsValidNumber(input);

        act.Should().BeFalse($"значение '{input}' должно быть НЕвалидным при precision={precision}, scale={scale}");
    }
}
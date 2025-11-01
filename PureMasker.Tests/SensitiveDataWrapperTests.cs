using Xunit;

namespace PureMasker.Tests;

public class SensitiveDataWrapperTests
{
    [Fact]
    public void WrapSensitiveData_CardNumber_MasksCorrectly()
    {
        var input = "4532148803436467";
        var result = SensitiveDataWrapper.WrapSensitiveData(input);
        Assert.Equal("4532********6467", result);
    }

    [Fact]
    public void WrapSensitiveData_Email_MasksCorrectly()
    {
        var input = "user@example.com";
        var result = SensitiveDataWrapper.WrapSensitiveData(input);
        Assert.Equal("us**@example.com", result);
    }

    [Fact]
    public void WrapSensitiveData_PhoneNumber_MasksCorrectly()
    {
        var input = "+380501234567";
        var result = SensitiveDataWrapper.WrapSensitiveData(input);
        Assert.Equal("+38********67", result);
    }

    [Fact]
    public void WrapSensitiveData_NonSensitiveData_ReturnsOriginal()
    {
        var input = "some random text";
        var result = SensitiveDataWrapper.WrapSensitiveData(input);
        Assert.Equal("some random text", result);
    }

    [Fact]
    public void WrapSensitiveData_WithCustomOptions_MasksCorrectly()
    {
        var input = "4532148803436467";
        var options = new MaskOptions { ShowFirst = 6, ShowLast = 4, MaskChar = '#' };
        var result = SensitiveDataWrapper.WrapSensitiveData(input, options);
        Assert.Equal("453214######6467", result);
    }

    [Fact]
    public void WrapSensitiveData_Null_ReturnsEmpty()
    {
        var result = SensitiveDataWrapper.WrapSensitiveData(null);
        Assert.Equal("", result);
    }

    [Fact]
    public void WrapSensitiveData_NumberObject_ConvertsAndMasks()
    {
        var input = 123456789012; // INN as number
        var result = SensitiveDataWrapper.WrapSensitiveData(input);
        Assert.Equal("123******012", result);
    }

    [Fact]
    public void WrapSensitiveData_IBAN_MasksCorrectly()
    {
        var input = "GB82WEST12345698765432";
        var result = SensitiveDataWrapper.WrapSensitiveData(input);
        Assert.Equal("GB82**************5432", result);
    }

    [Fact]
    public void WrapSensitiveData_Passport_MasksCorrectly()
    {
        var input = "KB123456";
        var result = SensitiveDataWrapper.WrapSensitiveData(input);
        Assert.Equal("KB****56", result);
    }

    [Fact]
    public void WrapSensitiveData_FullName_MasksCorrectly()
    {
        var input = "John Smith";
        var result = SensitiveDataWrapper.WrapSensitiveData(input);
        Assert.Equal("J********h", result);
    }

    [Theory]
    [InlineData("Іван Петренко")]
    [InlineData("Петро Іванович Сидоренко")]
    public void WrapSensitiveData_CyrillicFullName_MasksCorrectly(string input)
    {
        var result = SensitiveDataWrapper.WrapSensitiveData(input);
        Assert.StartsWith(input[0].ToString(), result);
        Assert.EndsWith(input[^1].ToString(), result);
        Assert.Contains("*", result);
    }
}

using Xunit;

namespace PureMasker.Tests;

public class SensitiveDataDetectorTests
{
    [Theory]
    [InlineData("GB82WEST12345698765432", SensitiveDataType.IBAN)]
    [InlineData("DE89370400440532013000", SensitiveDataType.IBAN)]
    [InlineData("UA213223130000026007233566001", SensitiveDataType.IBAN)]
    [InlineData("FR1420041010050500013M02606", SensitiveDataType.IBAN)]
    public void Detect_ValidIBAN_ReturnsIBAN(string input, SensitiveDataType expected)
    {
        var result = SensitiveDataDetector.Detect(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("1234567890123456", SensitiveDataType.BankAccount)]
    [InlineData("4532-1488-0343-6467", SensitiveDataType.CardNumber)]
    [InlineData("4532 1488 0343 6467", SensitiveDataType.CardNumber)]
    [InlineData("5425233430109903", SensitiveDataType.CardNumber)]
    public void Detect_ValidCardNumber_ReturnsCardNumber(string input, SensitiveDataType expected)
    {
        var result = SensitiveDataDetector.Detect(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("+380501234567", SensitiveDataType.PhoneNumber)]
    [InlineData("0501234567", SensitiveDataType.PhoneNumber)]
    [InlineData("+38 050 123 45 67", SensitiveDataType.PhoneNumber)]
    [InlineData("+38(050)1234567", SensitiveDataType.PhoneNumber)]
    public void Detect_ValidPhoneNumber_ReturnsPhoneNumber(string input, SensitiveDataType expected)
    {
        var result = SensitiveDataDetector.Detect(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("KB123456", SensitiveDataType.Passport)]
    [InlineData("КВ123456", SensitiveDataType.Passport)]
    [InlineData("kb123456", SensitiveDataType.Passport)]
    [InlineData("кв123456", SensitiveDataType.Passport)]
    public void Detect_ValidPassport_ReturnsPassport(string input, SensitiveDataType expected)
    {
        var result = SensitiveDataDetector.Detect(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("123456789012", SensitiveDataType.INN)]
    public void Detect_ValidINN_ReturnsINN(string input, SensitiveDataType expected)
    {
        var result = SensitiveDataDetector.Detect(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("john.doe@example.com", SensitiveDataType.Email)]
    [InlineData("user+tag@domain.co.uk", SensitiveDataType.Email)]
    [InlineData("test_user@test-domain.org", SensitiveDataType.Email)]
    public void Detect_ValidEmail_ReturnsEmail(string input, SensitiveDataType expected)
    {
        var result = SensitiveDataDetector.Detect(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("John Smith", SensitiveDataType.FullName)]
    [InlineData("Іван Петренко", SensitiveDataType.FullName)]
    [InlineData("Mary-Jane Watson", SensitiveDataType.FullName)]
    [InlineData("Петро Іванович Сидоренко", SensitiveDataType.FullName)]
    public void Detect_ValidFullName_ReturnsFullName(string input, SensitiveDataType expected)
    {
        var result = SensitiveDataDetector.Detect(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("12345678901234567890123456", SensitiveDataType.BankAccount)]
    public void Detect_ValidBankAccount_ReturnsBankAccount(string input, SensitiveDataType expected)
    {
        var result = SensitiveDataDetector.Detect(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("some random text")]
    [InlineData("123")]
    public void Detect_NonSensitiveData_ReturnsNone(string? input)
    {
        var result = SensitiveDataDetector.Detect(input);
        Assert.Equal(SensitiveDataType.None, result);
    }
}

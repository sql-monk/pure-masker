using Xunit;

namespace PureMasker.Tests;

public class DataMaskerTests
{
    [Fact]
    public void Mask_CardNumber_DefaultOptions_MasksCorrectly()
    {
        var input = "1234567890123456";
        var result = DataMasker.Mask(input, SensitiveDataType.CardNumber);
        Assert.Equal("1234********3456", result);
    }

    [Fact]
    public void Mask_CardNumber_CustomOptions_MasksCorrectly()
    {
        var input = "1234567890123456";
        var options = new MaskOptions { ShowFirst = 6, ShowLast = 4, MaskChar = '#' };
        var result = DataMasker.Mask(input, SensitiveDataType.CardNumber, options);
        Assert.Equal("123456######3456", result);
    }

    [Fact]
    public void Mask_IBAN_DefaultOptions_MasksCorrectly()
    {
        var input = "GB82WEST12345698765432";
        var result = DataMasker.Mask(input, SensitiveDataType.IBAN);
        Assert.Equal("GB82**************5432", result);
    }

    [Fact]
    public void Mask_PhoneNumber_DefaultOptions_MasksCorrectly()
    {
        var input = "+380501234567";
        var result = DataMasker.Mask(input, SensitiveDataType.PhoneNumber);
        Assert.Equal("+38********67", result);
    }

    [Fact]
    public void Mask_Email_DefaultOptions_MasksCorrectly()
    {
        var input = "john.doe@example.com";
        var result = DataMasker.Mask(input, SensitiveDataType.Email);
        Assert.Equal("jo******@example.com", result);
    }

    [Fact]
    public void Mask_Passport_DefaultOptions_MasksCorrectly()
    {
        var input = "KB123456";
        var result = DataMasker.Mask(input, SensitiveDataType.Passport);
        Assert.Equal("KB****56", result);
    }

    [Fact]
    public void Mask_INN_DefaultOptions_MasksCorrectly()
    {
        var input = "123456789012";
        var result = DataMasker.Mask(input, SensitiveDataType.INN);
        Assert.Equal("123******012", result);
    }

    [Fact]
    public void Mask_FullName_DefaultOptions_MasksCorrectly()
    {
        var input = "John Smith";
        var result = DataMasker.Mask(input, SensitiveDataType.FullName);
        Assert.Equal("J********h", result);
    }

    [Fact]
    public void Mask_ShortString_DoesNotCrash()
    {
        var input = "AB";
        var options = new MaskOptions { ShowFirst = 5, ShowLast = 5 };
        var result = DataMasker.Mask(input, SensitiveDataType.None, options);
        Assert.Equal("A*", result);
    }

    [Fact]
    public void Mask_EmptyString_ReturnsEmpty()
    {
        var input = "";
        var result = DataMasker.Mask(input, SensitiveDataType.None);
        Assert.Equal("", result);
    }

    [Fact]
    public void Mask_NullString_ReturnsEmpty()
    {
        string? input = null;
        var result = DataMasker.Mask(input!, SensitiveDataType.None);
        Assert.Equal("", result);
    }
}

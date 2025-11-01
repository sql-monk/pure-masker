namespace PureMasker;

public static class DataMasker
{
    public static string Mask(string input, SensitiveDataType dataType, MaskOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input ?? string.Empty;

        options ??= GetDefaultOptions(dataType);

        return ApplyMask(input, options);
    }

    private static MaskOptions GetDefaultOptions(SensitiveDataType dataType)
    {
        return dataType switch
        {
            SensitiveDataType.CardNumber => new MaskOptions { ShowFirst = 4, ShowLast = 4, MaskChar = '*' },
            SensitiveDataType.IBAN => new MaskOptions { ShowFirst = 4, ShowLast = 4, MaskChar = '*' },
            SensitiveDataType.PhoneNumber => new MaskOptions { ShowFirst = 3, ShowLast = 2, MaskChar = '*' },
            SensitiveDataType.Email => new MaskOptions { ShowFirst = 2, ShowLast = 0, MaskChar = '*' },
            SensitiveDataType.Passport => new MaskOptions { ShowFirst = 2, ShowLast = 2, MaskChar = '*' },
            SensitiveDataType.INN => new MaskOptions { ShowFirst = 3, ShowLast = 3, MaskChar = '*' },
            SensitiveDataType.BankAccount => new MaskOptions { ShowFirst = 4, ShowLast = 4, MaskChar = '*' },
            SensitiveDataType.FullName => new MaskOptions { ShowFirst = 1, ShowLast = 1, MaskChar = '*' },
            _ => MaskOptions.Default
        };
    }

    private static string ApplyMask(string input, MaskOptions options)
    {
        var trimmed = input.Trim();
        
        // Special handling for email
        if (trimmed.Contains('@'))
        {
            return MaskEmail(trimmed, options);
        }

        var showFirst = options.ShowFirst ?? 0;
        var showLast = options.ShowLast ?? 0;
        var maskChar = options.MaskChar;

        if (trimmed.Length <= showFirst + showLast)
        {
            // If string is too short, mask everything except first character
            return trimmed.Length > 0 ? trimmed[0] + new string(maskChar, trimmed.Length - 1) : trimmed;
        }

        var firstPart = trimmed.Substring(0, showFirst);
        var lastPart = trimmed.Substring(trimmed.Length - showLast);
        var middleLength = trimmed.Length - showFirst - showLast;

        return firstPart + new string(maskChar, middleLength) + lastPart;
    }

    private static string MaskEmail(string email, MaskOptions options)
    {
        var atIndex = email.IndexOf('@');
        if (atIndex <= 0)
            return email;

        var localPart = email.Substring(0, atIndex);
        var domainPart = email.Substring(atIndex);

        var showFirst = Math.Min(options.ShowFirst ?? 2, localPart.Length);
        var masked = localPart.Substring(0, showFirst);
        
        if (localPart.Length > showFirst)
        {
            masked += new string(options.MaskChar, localPart.Length - showFirst);
        }

        return masked + domainPart;
    }
}

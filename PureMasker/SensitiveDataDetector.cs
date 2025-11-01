using System.Text.RegularExpressions;

namespace PureMasker;

public static partial class SensitiveDataDetector
{
    // IBAN pattern: 2 letter country code + 2 check digits + up to 30 alphanumeric characters
    [GeneratedRegex(@"^[A-Z]{2}\d{2}[A-Z0-9]{1,30}$", RegexOptions.IgnoreCase)]
    private static partial Regex IBANPattern();

    // Bank account: typically 16-29 digits
    [GeneratedRegex(@"^\d{16,29}$")]
    private static partial Regex BankAccountPattern();

    // Phone number: various Ukrainian formats
    [GeneratedRegex(@"^(\+38)?[\s\-]?(\(?\d{3}\)?[\s\-]?\d{3}[\s\-]?\d{2}[\s\-]?\d{2}|\d{10})$")]
    private static partial Regex PhonePattern();

    // Ukrainian passport: KB999999 or КВ999999
    [GeneratedRegex(@"^[КкKk][ВвBb]\d{6}$")]
    private static partial Regex PassportPattern();

    // INN: exactly 12 digits
    [GeneratedRegex(@"^\d{12}$")]
    private static partial Regex INNPattern();

    // Card number: 13-19 digits, optionally grouped with spaces or dashes (not matching pure numbers with no separators)
    [GeneratedRegex(@"^[\d\s\-]{13,23}$")]
    private static partial Regex CardNumberPattern();

    // Email pattern
    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    private static partial Regex EmailPattern();

    // Full name: 2-4 words with letters (Cyrillic or Latin), at least 2 chars per word, starts with uppercase
    [GeneratedRegex(@"^[A-ZА-ЯІЇЄҐ][a-zа-яіїєґ]+(?:[\s\-][A-ZА-ЯІЇЄҐ][a-zа-яіїєґ]+){1,3}$")]
    private static partial Regex FullNamePattern();

    public static SensitiveDataType Detect(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return SensitiveDataType.None;

        var trimmed = input.Trim();

        // Order matters - more specific patterns first
        
        // Email (check early as it's distinct)
        if (EmailPattern().IsMatch(trimmed))
            return SensitiveDataType.Email;

        // IBAN
        if (IBANPattern().IsMatch(trimmed) && ValidateIBAN(trimmed))
            return SensitiveDataType.IBAN;

        // Passport
        if (PassportPattern().IsMatch(trimmed))
            return SensitiveDataType.Passport;

        // INN - exactly 12 digits
        if (INNPattern().IsMatch(trimmed))
            return SensitiveDataType.INN;

        // Card number with Luhn validation
        var digitsOnly = Regex.Replace(trimmed, @"[\s\-]", "");
        if (CardNumberPattern().IsMatch(trimmed) && digitsOnly.Length >= 13 && digitsOnly.Length <= 19)
        {
            // Check if it has separators (spaces or dashes) or validate with Luhn
            var hasSeparators = trimmed.Contains(' ') || trimmed.Contains('-');
            if (hasSeparators || ValidateLuhn(digitsOnly))
                return SensitiveDataType.CardNumber;
        }

        // Phone number
        if (PhonePattern().IsMatch(trimmed))
            return SensitiveDataType.PhoneNumber;

        // Bank account (after INN and card to avoid conflicts)
        if (BankAccountPattern().IsMatch(trimmed))
            return SensitiveDataType.BankAccount;

        // Full name (check last as it's less specific)
        if (FullNamePattern().IsMatch(trimmed))
            return SensitiveDataType.FullName;

        return SensitiveDataType.None;
    }

    private static bool ValidateLuhn(string cardNumber)
    {
        int sum = 0;
        bool alternate = false;

        for (int i = cardNumber.Length - 1; i >= 0; i--)
        {
            if (!char.IsDigit(cardNumber[i]))
                return false;

            int digit = cardNumber[i] - '0';

            if (alternate)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }

            sum += digit;
            alternate = !alternate;
        }

        return sum % 10 == 0;
    }

    private static bool ValidateIBAN(string iban)
    {
        if (iban.Length < 15 || iban.Length > 34)
            return false;

        // Move first 4 characters to end and convert to numbers
        var rearranged = iban.Substring(4) + iban.Substring(0, 4);
        var numericString = "";

        foreach (char c in rearranged)
        {
            if (char.IsDigit(c))
                numericString += c;
            else if (char.IsLetter(c))
                numericString += (char.ToUpper(c) - 'A' + 10).ToString();
            else
                return false;
        }

        // Calculate mod 97
        return Mod97(numericString) == 1;
    }

    private static int Mod97(string number)
    {
        var remainder = 0;
        foreach (char digit in number)
        {
            remainder = (remainder * 10 + (digit - '0')) % 97;
        }
        return remainder;
    }
}

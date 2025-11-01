using System;
using PureMasker;

namespace PureMasker.Examples;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== PureMasker Examples ===\n");

        // Example 1: Card Number
        Console.WriteLine("1. Card Number:");
        var cardNumber = "4532148803436467";
        Console.WriteLine($"   Original: {cardNumber}");
        Console.WriteLine($"   Masked:   {SensitiveDataWrapper.WrapSensitiveData(cardNumber)}\n");

        // Example 2: Email
        Console.WriteLine("2. Email:");
        var email = "user@example.com";
        Console.WriteLine($"   Original: {email}");
        Console.WriteLine($"   Masked:   {SensitiveDataWrapper.WrapSensitiveData(email)}\n");

        // Example 3: Phone Number
        Console.WriteLine("3. Phone Number:");
        var phone = "+380501234567";
        Console.WriteLine($"   Original: {phone}");
        Console.WriteLine($"   Masked:   {SensitiveDataWrapper.WrapSensitiveData(phone)}\n");

        // Example 4: Ukrainian Passport
        Console.WriteLine("4. Ukrainian Passport:");
        var passport = "КВ123456";
        Console.WriteLine($"   Original: {passport}");
        Console.WriteLine($"   Masked:   {SensitiveDataWrapper.WrapSensitiveData(passport)}\n");

        // Example 5: IBAN
        Console.WriteLine("5. IBAN:");
        var iban = "GB82WEST12345698765432";
        Console.WriteLine($"   Original: {iban}");
        Console.WriteLine($"   Masked:   {SensitiveDataWrapper.WrapSensitiveData(iban)}\n");

        // Example 6: INN (Tax Number)
        Console.WriteLine("6. INN (Tax Number):");
        var inn = "123456789012";
        Console.WriteLine($"   Original: {inn}");
        Console.WriteLine($"   Masked:   {SensitiveDataWrapper.WrapSensitiveData(inn)}\n");

        // Example 7: Full Name (Cyrillic)
        Console.WriteLine("7. Full Name (Cyrillic):");
        var name = "Іван Петренко";
        Console.WriteLine($"   Original: {name}");
        Console.WriteLine($"   Masked:   {SensitiveDataWrapper.WrapSensitiveData(name)}\n");

        // Example 8: Full Name (Latin)
        Console.WriteLine("8. Full Name (Latin):");
        var nameEn = "John Smith";
        Console.WriteLine($"   Original: {nameEn}");
        Console.WriteLine($"   Masked:   {SensitiveDataWrapper.WrapSensitiveData(nameEn)}\n");

        // Example 9: Custom Masking Options
        Console.WriteLine("9. Card Number with Custom Options:");
        var customOptions = new MaskOptions 
        { 
            ShowFirst = 6, 
            ShowLast = 4, 
            MaskChar = '#' 
        };
        Console.WriteLine($"   Original: {cardNumber}");
        Console.WriteLine($"   Masked:   {SensitiveDataWrapper.WrapSensitiveData(cardNumber, customOptions)}\n");

        // Example 10: Non-sensitive Data
        Console.WriteLine("10. Regular Text (may be masked if it looks like a name):");
        var normalText = "Hello World";
        Console.WriteLine($"    Original: {normalText}");
        Console.WriteLine($"    Masked:   {SensitiveDataWrapper.WrapSensitiveData(normalText)}");
        Console.WriteLine($"    Note: Capitalized words may be treated as names for safety\n");

        // Example 11: Truly non-sensitive data
        Console.WriteLine("11. Truly Non-sensitive Data:");
        var plainText = "this is lowercase text";
        Console.WriteLine($"    Original: {plainText}");
        Console.WriteLine($"    Masked:   {SensitiveDataWrapper.WrapSensitiveData(plainText)}\n");

        Console.WriteLine("Press any key to exit...");
        if (Console.IsInputRedirected)
        {
            Console.WriteLine("(Running in non-interactive mode, exiting automatically)");
        }
        else
        {
            Console.ReadKey();
        }
    }
}

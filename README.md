# PureMasker

A high-performance .NET library for detecting and masking sensitive data in strings. Designed for safe logging and data handling.

## Features

PureMasker can automatically detect and mask the following types of sensitive data:

- **IBAN** - International Bank Account Numbers
- **Bank Account Numbers** - 16-29 digit account numbers
- **Phone Numbers** - Ukrainian phone numbers in various formats
- **Passport Numbers** - Ukrainian passport format (KB999999, КВ999999)
- **INN** - Individual Tax Numbers (12 digits)
- **Card Numbers** - Credit/debit card numbers with Luhn validation
- **Full Names** - Names in both Cyrillic and Latin alphabets
- **Email Addresses**

## Installation

```bash
dotnet add package PureMasker
```

## Usage

### Basic Usage

```csharp
using PureMasker;

// Automatically detect and mask sensitive data
var maskedCard = SensitiveDataWrapper.WrapSensitiveData("4532148803436467");
// Output: "4532********6467"

var maskedEmail = SensitiveDataWrapper.WrapSensitiveData("user@example.com");
// Output: "us**@example.com"

var maskedPhone = SensitiveDataWrapper.WrapSensitiveData("+380501234567");
// Output: "+38********67"

// Non-sensitive data passes through unchanged
var text = SensitiveDataWrapper.WrapSensitiveData("Hello World");
// Output: "Hello World"
```

### Custom Masking Options

```csharp
using PureMasker;

var options = new MaskOptions 
{
    ShowFirst = 6,
    ShowLast = 4,
    MaskChar = '#'
};

var masked = SensitiveDataWrapper.WrapSensitiveData("4532148803436467", options);
// Output: "453214######6467"
```

### Default Masking Rules by Data Type

Different types of sensitive data have different default masking rules:

| Data Type | Show First | Show Last | Mask Char |
|-----------|------------|-----------|-----------|
| Card Number | 4 | 4 | * |
| IBAN | 4 | 4 | * |
| Phone Number | 3 | 2 | * |
| Email | 2 | 0 | * |
| Passport | 2 | 2 | * |
| INN | 3 | 3 | * |
| Bank Account | 4 | 4 | * |
| Full Name | 1 | 1 | * |

### Examples

```csharp
using PureMasker;

// Ukrainian passport
var passport = SensitiveDataWrapper.WrapSensitiveData("КВ123456");
// Output: "КВ****56"

// IBAN
var iban = SensitiveDataWrapper.WrapSensitiveData("GB82WEST12345698765432");
// Output: "GB82**************5432"

// Full name (Cyrillic)
var name = SensitiveDataWrapper.WrapSensitiveData("Іван Петренко");
// Output: "І***********о"

// INN (Tax Number)
var inn = SensitiveDataWrapper.WrapSensitiveData("123456789012");
// Output: "123******012"
```

## Performance

PureMasker is optimized for high-performance scenarios where masking may be called frequently:

- Uses compiled regular expressions (source generators)
- Minimal allocations
- Fast pattern matching
- Efficient string operations

## API Reference

### `SensitiveDataWrapper`

#### `WrapSensitiveData(object? sensitive)`
Detects and masks sensitive data using default masking rules.

#### `WrapSensitiveData(object? sensitive, MaskOptions? maskOptions)`
Detects and masks sensitive data using custom masking options.

### `MaskOptions`

Properties:
- `ShowFirst` (int?) - Number of characters to show at the beginning
- `ShowLast` (int?) - Number of characters to show at the end
- `MaskChar` (char) - Character to use for masking (default: '*')

### `SensitiveDataType` Enum

- `None` - Not sensitive data
- `IBAN`
- `BankAccount`
- `PhoneNumber`
- `Passport`
- `INN`
- `CardNumber`
- `FullName`
- `Email`

## License

MIT License

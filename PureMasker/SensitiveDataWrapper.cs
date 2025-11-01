namespace PureMasker;

public static class SensitiveDataWrapper
{
    public static string WrapSensitiveData(object? sensitive)
    {
        return WrapSensitiveData(sensitive, null);
    }

    public static string WrapSensitiveData(object? sensitive, MaskOptions? maskOptions)
    {
        if (sensitive == null)
            return string.Empty;

        var stringValue = sensitive.ToString();
        if (string.IsNullOrWhiteSpace(stringValue))
            return stringValue ?? string.Empty;

        var dataType = SensitiveDataDetector.Detect(stringValue);

        if (dataType == SensitiveDataType.None)
            return stringValue;

        return DataMasker.Mask(stringValue, dataType, maskOptions);
    }
}

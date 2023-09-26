namespace HamedStack.CSBridge.PredefinedAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class TsDefaultValueAttribute : Attribute
{
    public TsDefaultValueAttribute(string value, bool isStringValue = false)
    {
        Value = value;
        IsStringType = isStringValue;
    }

    public bool IsStringType { get; }
    public string Value { get; }
}
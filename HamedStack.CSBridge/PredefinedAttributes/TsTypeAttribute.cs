namespace HamedStack.CSBridge.PredefinedAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class TsTypeAttribute : Attribute
{
    public TsTypeAttribute(string type)
    {
        Type = type;
    }

    public string Type { get; }
}
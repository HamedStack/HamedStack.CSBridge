namespace HamedStack.CSBridge.PredefinedAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class TsTypeUnionsAttribute : Attribute
{
    public TsTypeUnionsAttribute(params string[] types)
    {
        Types = types;
    }

    public string[] Types { get; }
}
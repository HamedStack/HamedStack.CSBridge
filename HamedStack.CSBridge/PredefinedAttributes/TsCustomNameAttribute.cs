namespace HamedStack.CSBridge.PredefinedAttributes;

[AttributeUsage(AttributeTargets.Property)]
public class TsCustomNameAttribute : Attribute
{
    public TsCustomNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
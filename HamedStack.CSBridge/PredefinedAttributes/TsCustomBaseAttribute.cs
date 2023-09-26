namespace HamedStack.CSBridge.PredefinedAttributes;

[AttributeUsage(AttributeTargets.Class)]
public class TsCustomBaseAttribute : Attribute
{
    public TsCustomBaseAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
namespace HamedStack.CSBridge.PredefinedAttributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum)]
public class ExportTsAttribute : Attribute
{
    public ExportTsAttribute(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
        Name = name;
    }

    public string Name { get; }
}
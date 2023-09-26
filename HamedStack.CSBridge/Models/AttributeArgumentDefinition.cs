namespace HamedStack.CSBridge.Models;

public class AttributeArgumentDefinition
{
    public AttributeArgumentDefinition()
    {
        Values = new List<string>();
    }

    public string Name { get; set; } = null!;
    public List<string> Values { get; set; }
}
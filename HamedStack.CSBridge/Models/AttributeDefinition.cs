namespace HamedStack.CSBridge.Models;

public class AttributeDefinition
{
    public List<AttributeArgumentDefinition>? Arguments { get; set; }
    public string Name { get; set; } = null!;
    public string Target { get; set; } = null!;
    public string TargetName { get; set; } = null!;
}
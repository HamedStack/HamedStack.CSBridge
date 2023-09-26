namespace HamedStack.CSBridge.Models;

public class MemberDefinition
{
    public List<string>? AccessModifiers { get; set; }
    public string? EnumName { get; set; }
    public long? EnumValue { get; set; }
    public List<string>? GenericTypes { get; set; }
    public bool IsEnumerable { get; set; }
    public bool IsEnumMember { get; set; }
    public bool IsGeneric { get; set; }
    public bool IsNullable { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public List<string>? TypeInterfaces { get; set; }
}
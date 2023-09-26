namespace HamedStack.CSBridge.Models;

public class TypeDefinition
{
    public TypeDefinition()
    {
        AccessModifiers = new List<string>();
        PredefinedAttributes = new List<AttributeDefinition>();
    }

    public List<string> AccessModifiers { get; set; }
    public List<string>? BaseGenericType { get; set; }
    public string? BaseType { get; set; }
    public List<GenericConstraintDefinition>? GenericConstraints { get; set; }
    public List<string>? GenericType { get; set; }
    public List<string>? InnerInterfaces { get; set; }
    public List<string>? Interfaces { get; set; }
    public bool IsGeneric { get; set; }
    public string Kind { get; set; } = null!;
    public List<MemberDefinition>? Members { get; set; }
    public string Name { get; set; } = null!;
    public List<AttributeDefinition> PredefinedAttributes { get; set; }
}
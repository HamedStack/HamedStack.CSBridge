namespace HamedStack.CSBridge.Models;

public class GenericConstraintDefinition
{
    public GenericConstraintDefinition()
    {
        Members = new List<string>();
    }

    public List<string> Members { get; set; }
    public string Name { get; set; } = null!;
}
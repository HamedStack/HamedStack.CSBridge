namespace HamedStack.CSBridge.PredefinedAttributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
public class ExportTsStructAttribute : ExportTsAttribute
{
    public ExportTsStructAttribute(string name) : base(name)
    {
    }
}
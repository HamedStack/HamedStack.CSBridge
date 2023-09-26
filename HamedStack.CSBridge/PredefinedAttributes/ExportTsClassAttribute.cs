namespace HamedStack.CSBridge.PredefinedAttributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
public class ExportTsClassAttribute : ExportTsAttribute
{
    public ExportTsClassAttribute(string name) : base(name)
    {
    }
}
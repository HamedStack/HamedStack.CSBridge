namespace HamedStack.CSBridge.PredefinedAttributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
public class ExportTsInterfaceAttribute : ExportTsAttribute
{
    public ExportTsInterfaceAttribute(string name) : base(name)
    {
    }
}
namespace HamedStack.CSBridge.PredefinedAttributes;

[AttributeUsage(AttributeTargets.Enum)]
public class ExportTsEnumAttribute : ExportTsAttribute
{
    public ExportTsEnumAttribute(string name) : base(name)
    {
    }
}
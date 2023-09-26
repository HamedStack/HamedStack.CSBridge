using HamedStack.CSBridge.Enums;

namespace HamedStack.CSBridge.Models;

public class GeneratorSettings
{
    public bool GenerateConstructor { get; set; }
    public TypeScriptDateType GenerateDateTypeAs { get; set; } = TypeScriptDateType.StringOrDate;
    public bool GenerateForAllAccessModifiers { get; set; }
    public bool GenerateInterface { get; set; } = true;
    public bool MakeAllPropertiesOptional { get; set; }
    public bool UseAny { get; set; }
    public bool UsePredefinedTypes { get; set; }
}
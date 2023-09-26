using System.Text;
using HamedStack.CSBridge.Enums;
using HamedStack.CSBridge.Models;
using HamedStack.CSBridge.PredefinedAttributes;

namespace HamedStack.CSBridge.Helpers;

internal static class TypeDefinitionExtensions
{
    internal static string GenerateConstructor(this TypeDefinition typeDefinition, int startingTab = 0)
    {
        var tabs = startingTab < 0 ? string.Empty : new string('\t', startingTab);

        if (typeDefinition.Members is not { Count: > 0 } || typeDefinition.Kind is nameof(Kind.Enum) or nameof(Kind.Interface))
            return string.Empty;

        var sb = new StringBuilder();
        sb.AppendLine($"{tabs}constructor({string.Join(", ", typeDefinition.Members.Select(m => $"{m.Name?.ConvertTextTo(HumanizerInflectorType.Camelize)}{(m.IsNullable ? "?" : null)}: {TypescriptUtility.FromCSharpStringType(m.Type, m.TypeInterfaces?.ToArray(), m.GenericTypes?.ToArray())}"))}) {{");
        foreach (var member in typeDefinition.Members)
        {
            var name = member.Name?.ConvertTextTo(HumanizerInflectorType.Camelize);
            sb.AppendLine($"\t{tabs}this.{name} = {name};");
        }
        sb.AppendLine($"{tabs}}}");
        return sb.ToString();
    }

    internal static string GenerateInterface(this TypeDefinition typeDefinition, int startingTab = 0, bool skipExtends = false, bool exportDefault = false)
    {
        var tabs = startingTab < 0 ? string.Empty : new string('\t', startingTab);

        if (typeDefinition.Members is not { Count: > 0 } || typeDefinition.Kind is nameof(Kind.Enum))
            return string.Empty;

        var sb = new StringBuilder();

        var export = exportDefault ? "export default" : "export";
        var extends = skipExtends ? string.Empty : typeDefinition.BaseType is null ? string.Empty : $"extends {TypescriptUtility.FromCSharpStringType(typeDefinition.BaseType, null, typeDefinition.BaseGenericType?.ToArray(), true)}";

        sb.AppendLine($"{tabs}{export} interface {typeDefinition.Name} {extends} {{");
        foreach (var member in typeDefinition.Members)
        {
            var name = member.Name?.ConvertTextTo(HumanizerInflectorType.Camelize);
            sb.AppendLine($"\t{tabs}{name}{(member.IsNullable ? "?" : "")}: {TypescriptUtility.FromCSharpStringType(member.Type, member.TypeInterfaces?.ToArray(), member.GenericTypes?.ToArray())};");
        }
        sb.AppendLine($"{tabs}}}");
        return sb.ToString();
    }

    internal static bool ValidateTypeDefinitions(this IEnumerable<TypeDefinition> typeDefinitions, out IList<string> errors)
    {
        errors = new List<string>();
        var definitions = typeDefinitions.ToList();
        foreach (var typeDefinition in definitions)
        {
            var hasExportTsAttribute = typeDefinition.PredefinedAttributes.HasExportTsAttribute();
            if (!hasExportTsAttribute)
            {
                errors.Add($"You should add one of ExportTs attributes on top of your '{typeDefinition.Name}' type.");
            }
        }

        var allExports = definitions.SelectMany(x => x.PredefinedAttributes).Where(x => x.Name is
            nameof(ExportTsAttribute)
            or nameof(ExportTsClassAttribute)
            or nameof(ExportTsStructAttribute)
            or nameof(ExportTsInterfaceAttribute)
            or nameof(ExportTsEnumAttribute));

        var allNames = allExports
                .Where(x => x.Arguments != null)
                .SelectMany(x => x.Arguments!)
                .Select(x => x.Values[0])
                .ToList()
            ;

        foreach (var duplicate in allNames.FindDuplicates(x => x.ToLowerInvariant()))
        {
            errors.Add($"By a case-insensitive check, '{duplicate}' type is duplicate.");
        }

        return !errors.Any();
    }
}
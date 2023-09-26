using HamedStack.CSBridge.Models;
using HamedStack.CSBridge.PredefinedAttributes;

namespace HamedStack.CSBridge.Helpers;

internal static class AttributeDefinitionExtensions
{
    internal static bool HasDuplicateExportTsName(this IEnumerable<AttributeDefinition> attributeDefinitions)
    {
        var definitions = attributeDefinitions.ToList();
        var hasExportTsAttribute = definitions.Where(x => x.Name is
            nameof(ExportTsAttribute)
            or nameof(ExportTsClassAttribute)
            or nameof(ExportTsStructAttribute)
            or nameof(ExportTsInterfaceAttribute)
            or nameof(ExportTsEnumAttribute));
        return true;
    }

    internal static bool HasExportTsAttribute(this IEnumerable<AttributeDefinition> attributeDefinitions)
    {
        return attributeDefinitions.Any(x => x.Name is
            nameof(ExportTsAttribute)
            or nameof(ExportTsClassAttribute)
            or nameof(ExportTsStructAttribute)
            or nameof(ExportTsInterfaceAttribute)
            or nameof(ExportTsEnumAttribute));
    }
}
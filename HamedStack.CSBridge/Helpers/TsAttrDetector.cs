using System.Reflection;
using HamedStack.CSBridge.Models;
using HamedStack.CSBridge.PredefinedAttributes;

namespace HamedStack.CSBridge.Helpers;

internal static class TsAttrDetector
{
    internal static AttributeDefinition? ExportTsAttributeDetector(this Type type)
    {
        var exportTs = type.GetCustomAttribute<ExportTsAttribute>();
        if (exportTs == null) return null;
        var args = new AttributeArgumentDefinition()
        {
            Name = nameof(ExportTsAttribute.Name),
            Values = new List<string> { exportTs.Name }
        };
        return new AttributeDefinition
        {
            Name = nameof(ExportTsAttribute),
            TargetName = type.GetName(),
            Target = type.GetKind().ToString(),
            Arguments = new List<AttributeArgumentDefinition> { args }
        };
    }

    internal static AttributeDefinition? ExportTsClassAttributeDetector(this Type type)
    {
        var exportTs = type.GetCustomAttribute<ExportTsClassAttribute>();
        if (exportTs == null) return null;
        var args = new AttributeArgumentDefinition()
        {
            Name = nameof(ExportTsAttribute.Name),
            Values = new List<string> { exportTs.Name }
        };
        return new AttributeDefinition
        {
            Name = nameof(ExportTsClassAttribute),
            TargetName = type.GetName(),
            Target = type.GetKind().ToString(),
            Arguments = new List<AttributeArgumentDefinition> { args }
        };
    }

    internal static AttributeDefinition? ExportTsEnumAttributeDetector(this Type type)
    {
        var exportTs = type.GetCustomAttribute<ExportTsEnumAttribute>();
        if (exportTs == null) return null;
        var args = new AttributeArgumentDefinition()
        {
            Name = nameof(ExportTsAttribute.Name),
            Values = new List<string> { exportTs.Name }
        };
        return new AttributeDefinition
        {
            Name = nameof(ExportTsEnumAttribute),
            TargetName = type.GetName(),
            Target = type.GetKind().ToString(),
            Arguments = new List<AttributeArgumentDefinition> { args }
        };
    }

    internal static AttributeDefinition? ExportTsInterfaceAttributeDetector(this Type type)
    {
        var exportTs = type.GetCustomAttribute<ExportTsInterfaceAttribute>();
        if (exportTs == null) return null;
        var args = new AttributeArgumentDefinition()
        {
            Name = nameof(ExportTsAttribute.Name),
            Values = new List<string> { exportTs.Name }
        };
        return new AttributeDefinition
        {
            Name = nameof(ExportTsInterfaceAttribute),
            TargetName = type.GetName(),
            Target = type.GetKind().ToString(),
            Arguments = new List<AttributeArgumentDefinition> { args }
        };
    }

    internal static AttributeDefinition? ExportTsStructAttributeDetector(this Type type)
    {
        var exportTs = type.GetCustomAttribute<ExportTsStructAttribute>();
        if (exportTs == null) return null;
        var args = new AttributeArgumentDefinition()
        {
            Name = nameof(ExportTsAttribute.Name),
            Values = new List<string> { exportTs.Name }
        };
        return new AttributeDefinition
        {
            Name = nameof(ExportTsStructAttribute),
            TargetName = type.GetName(),
            Target = type.GetKind().ToString(),
            Arguments = new List<AttributeArgumentDefinition> { args }
        };
    }

    internal static IList<AttributeDefinition> GetPredefinedAttributes(this Type type)
    {
        var attributes = new List<AttributeDefinition>();

        var exportTsClass = type.ExportTsClassAttributeDetector();
        if (exportTsClass is not null)
        {
            attributes.Add(exportTsClass);
        }

        var exportTsEnum = type.ExportTsEnumAttributeDetector();
        if (exportTsEnum is not null)
        {
            attributes.Add(exportTsEnum);
        }

        var exportTsInterface = type.ExportTsInterfaceAttributeDetector();
        if (exportTsInterface is not null)
        {
            attributes.Add(exportTsInterface);
        }

        var exportTsStruct = type.ExportTsStructAttributeDetector();
        if (exportTsStruct is not null)
        {
            attributes.Add(exportTsStruct);
        }

        var exportTs = type.ExportTsAttributeDetector();
        if (exportTs is not null && exportTsClass == null && exportTsEnum == null && exportTsInterface == null && exportTsStruct == null)
        {
            attributes.Add(exportTs);
        }

        var tsCustomBase = type.TsCustomBaseAttributeDetector();
        if (tsCustomBase is not null)
        {
            attributes.Add(tsCustomBase);
        }

        var tsDefaultExport = type.TsDefaultExportAttributeDetector();
        if (tsDefaultExport is not null)
        {
            attributes.Add(tsDefaultExport);
        }

        var tsIgnoreBase = type.TsIgnoreBaseAttributeDetector();
        if (tsIgnoreBase is not null)
        {
            attributes.Add(tsIgnoreBase);
        }

        var tsStringEnum = type.TsStringEnumAttributeDetector();
        if (tsStringEnum is not null)
        {
            attributes.Add(tsStringEnum);
        }

        foreach (var prop in type.GetProperties())
        {
            var tsTypeUnion = prop.TsTypeUnionsAttributeDetector();
            if (tsTypeUnion is not null)
            {
                attributes.Add(tsTypeUnion);
            }

            var exportTsName = prop.TsCustomNameAttributeDetector();
            if (exportTsName is not null)
            {
                attributes.Add(exportTsName);
            }

            var tsDefaultValue = prop.TsDefaultValueAttributeDetector();
            if (tsDefaultValue is not null)
            {
                attributes.Add(tsDefaultValue);
            }

            var tsIgnore = prop.TsIgnoreAttributeDetector();
            if (tsIgnore is not null)
            {
                attributes.Add(tsIgnore);
            }

            var tsNotNull = prop.TsNotNullAttributeDetector();
            if (tsNotNull is not null)
            {
                attributes.Add(tsNotNull);
            }

            var tsNotOptional = prop.TsNotOptionalAttributeDetector();
            if (tsNotOptional is not null)
            {
                attributes.Add(tsNotOptional);
            }

            var tsNotReadonly = prop.TsNotReadonlyAttributeDetector();
            if (tsNotReadonly is not null)
            {
                attributes.Add(tsNotReadonly);
            }

            var tsNotStatic = prop.TsNotStaticAttributeDetector();
            if (tsNotStatic is not null)
            {
                attributes.Add(tsNotStatic);
            }

            var tsNull = prop.TsNullAttributeDetector();
            if (tsNull is not null)
            {
                attributes.Add(tsNull);
            }

            var tsOptional = prop.TsOptionalAttributeDetector();
            if (tsOptional is not null)
            {
                attributes.Add(tsOptional);
            }

            var tsReadonly = prop.TsReadonlyAttributeDetector();
            if (tsReadonly is not null)
            {
                attributes.Add(tsReadonly);
            }

            var tsStatic = prop.TsStaticAttributeDetector();
            if (tsStatic is not null)
            {
                attributes.Add(tsStatic);
            }

            var tsType = prop.TsTypeAttributeDetector();
            if (tsType is not null)
            {
                attributes.Add(tsType);
            }
        }

        return attributes;
    }

    internal static AttributeDefinition? TsCustomBaseAttributeDetector(this Type type)
    {
        var tsCustomBase = type.GetCustomAttribute<TsCustomBaseAttribute>();
        if (tsCustomBase == null) return null;
        var args = new List<AttributeArgumentDefinition>
        {
            new()
            {
                Name = nameof(TsCustomNameAttribute.Name),
                Values = new List<string> { tsCustomBase.Name}
            }
        };
        return new AttributeDefinition
        {
            Name = nameof(TsCustomBaseAttribute),
            TargetName = type.GetName(),
            Target = type.GetKind().ToString(),
            Arguments = args
        };
    }

    internal static AttributeDefinition? TsCustomNameAttributeDetector(this MemberInfo type)
    {
        var tsCustomName = type.GetCustomAttribute<TsCustomNameAttribute>();
        if (tsCustomName == null) return null;
        var args = new List<AttributeArgumentDefinition>
        {
            new()
            {
                Name = nameof(TsCustomNameAttribute.Name),
                Values = new List<string> { tsCustomName.Name}
            }
        };
        return new AttributeDefinition
        {
            Name = nameof(TsCustomNameAttribute),
            TargetName = type.Name,
            Target = nameof(AttributeTargets.Property),
            Arguments = args
        };
    }

    internal static AttributeDefinition? TsDefaultExportAttributeDetector(this Type type)
    {
        var tsDefaultExport = type.GetCustomAttribute<TsDefaultExportAttribute>();
        if (tsDefaultExport == null) return null;
        return new AttributeDefinition
        {
            Name = nameof(TsDefaultExportAttribute),
            TargetName = type.GetName(),
            Target = type.GetKind().ToString(),
        };
    }

    internal static AttributeDefinition? TsDefaultValueAttributeDetector(this MemberInfo type)
    {
        var tsDefaultValue = type.GetCustomAttribute<TsDefaultValueAttribute>();
        if (tsDefaultValue == null) return null;
        var args = new List<AttributeArgumentDefinition>
        {
            new()
            {
                Name = nameof(TsDefaultValueAttribute.Value),
                Values = new List<string> { tsDefaultValue.Value}
            },
            new()
            {
                Name = nameof(TsDefaultValueAttribute.IsStringType),
                Values = new List<string> { tsDefaultValue.IsStringType.ToString()}
            }
        };
        return new AttributeDefinition
        {
            Name = nameof(TsDefaultValueAttribute),
            TargetName = type.Name,
            Target = nameof(AttributeTargets.Property),
            Arguments = args
        };
    }

    internal static AttributeDefinition? TsIgnoreAttributeDetector(this MemberInfo type)
    {
        var unionTypes = type.GetCustomAttribute<TsIgnoreAttribute>();
        if (unionTypes == null) return null;

        return new AttributeDefinition
        {
            Name = nameof(TsIgnoreAttribute),
            TargetName = type.Name,
            Target = nameof(AttributeTargets.Property),
        };
    }

    internal static AttributeDefinition? TsIgnoreBaseAttributeDetector(this Type type)
    {
        var tsDefaultExport = type.GetCustomAttribute<TsIgnoreBaseAttribute>();
        if (tsDefaultExport == null) return null;
        return new AttributeDefinition
        {
            Name = nameof(TsIgnoreBaseAttribute),
            TargetName = type.GetName(),
            Target = type.GetKind().ToString(),
        };
    }

    internal static AttributeDefinition? TsNotNullAttributeDetector(this MemberInfo type)
    {
        var unionTypes = type.GetCustomAttribute<TsNotNullAttribute>();
        if (unionTypes == null) return null;

        return new AttributeDefinition
        {
            Name = nameof(TsNotNullAttribute),
            TargetName = type.Name,
            Target = nameof(AttributeTargets.Property),
        };
    }

    internal static AttributeDefinition? TsNotOptionalAttributeDetector(this MemberInfo type)
    {
        var unionTypes = type.GetCustomAttribute<TsNotOptionalAttribute>();
        if (unionTypes == null) return null;

        return new AttributeDefinition
        {
            Name = nameof(TsNotOptionalAttribute),
            TargetName = type.Name,
            Target = nameof(AttributeTargets.Property),
        };
    }

    internal static AttributeDefinition? TsNotReadonlyAttributeDetector(this MemberInfo type)
    {
        var unionTypes = type.GetCustomAttribute<TsNotReadonlyAttribute>();
        if (unionTypes == null) return null;

        return new AttributeDefinition
        {
            Name = nameof(TsNotReadonlyAttribute),
            TargetName = type.Name,
            Target = nameof(AttributeTargets.Property),
        };
    }

    internal static AttributeDefinition? TsNotStaticAttributeDetector(this MemberInfo type)
    {
        var unionTypes = type.GetCustomAttribute<TsNotStaticAttribute>();
        if (unionTypes == null) return null;

        return new AttributeDefinition
        {
            Name = nameof(TsNotStaticAttribute),
            TargetName = type.Name,
            Target = nameof(AttributeTargets.Property),
        };
    }

    internal static AttributeDefinition? TsNullAttributeDetector(this MemberInfo type)
    {
        var unionTypes = type.GetCustomAttribute<TsNullAttribute>();
        if (unionTypes == null) return null;

        return new AttributeDefinition
        {
            Name = nameof(TsNullAttribute),
            TargetName = type.Name,
            Target = nameof(AttributeTargets.Property),
        };
    }

    internal static AttributeDefinition? TsOptionalAttributeDetector(this MemberInfo type)
    {
        var unionTypes = type.GetCustomAttribute<TsOptionalAttribute>();
        if (unionTypes == null) return null;

        return new AttributeDefinition
        {
            Name = nameof(TsOptionalAttribute),
            TargetName = type.Name,
            Target = nameof(AttributeTargets.Property),
        };
    }

    internal static AttributeDefinition? TsReadonlyAttributeDetector(this MemberInfo type)
    {
        var unionTypes = type.GetCustomAttribute<TsReadonlyAttribute>();
        if (unionTypes == null) return null;

        return new AttributeDefinition
        {
            Name = nameof(TsReadonlyAttribute),
            TargetName = type.Name,
            Target = nameof(AttributeTargets.Property),
        };
    }

    internal static AttributeDefinition? TsStaticAttributeDetector(this MemberInfo type)
    {
        var unionTypes = type.GetCustomAttribute<TsStaticAttribute>();
        if (unionTypes == null) return null;

        return new AttributeDefinition
        {
            Name = nameof(TsStaticAttribute),
            TargetName = type.Name,
            Target = nameof(AttributeTargets.Property),
        };
    }

    internal static AttributeDefinition? TsStringEnumAttributeDetector(this Type type)
    {
        var tsDefaultExport = type.GetCustomAttribute<TsStringEnumAttribute>();
        if (tsDefaultExport == null) return null;
        return new AttributeDefinition
        {
            Name = nameof(TsStringEnumAttribute),
            TargetName = type.GetName(),
            Target = type.GetKind().ToString(),
        };
    }

    internal static AttributeDefinition? TsTypeAttributeDetector(this MemberInfo type)
    {
        var unionTypes = type.GetCustomAttribute<TsTypeAttribute>();
        if (unionTypes == null) return null;

        var args = new List<AttributeArgumentDefinition>
        {
            new()
            {
                Name = nameof(TsTypeAttribute.Type),
                Values = new List<string> { unionTypes.Type}
            }
        };
        return new AttributeDefinition
        {
            Name = nameof(TsTypeAttribute),
            TargetName = type.Name,
            Target = nameof(AttributeTargets.Property),
            Arguments = args
        };
    }

    internal static AttributeDefinition? TsTypeUnionsAttributeDetector(this MemberInfo type)
    {
        var unionTypes = type.GetCustomAttribute<TsTypeUnionsAttribute>()?.Types;
        if (unionTypes is not { Length: > 0 }) return null;

        var args = new List<AttributeArgumentDefinition>
        {
            new()
            {
                Name = nameof(TsTypeUnionsAttribute.Types),
                Values = unionTypes.ToList()
            }
        };
        return new AttributeDefinition
        {
            Name = nameof(TsTypeUnionsAttribute),
            TargetName = type.Name,
            Target = nameof(AttributeTargets.Property),
            Arguments = args
        };
    }
}
using HamedStack.CSBridge.Models;

namespace HamedStack.CSBridge.Helpers;

internal static class ClassTypeDefinitionHelper
{
    internal static TypeDefinition GetClassOrInterfaceOrStructTypeDefinition(this Type type)
    {
        var typeDefinition = new TypeDefinition();
        var genericConstraints = new List<GenericConstraintDefinition>();
        foreach (var genericArgument in type.GetGenericConstraints())
        {
            var genericConstraint = new GenericConstraintDefinition
            {
                Name = genericArgument.Key,
                Members = genericArgument.Value
            };
            genericConstraints.Add(genericConstraint);
        }
        typeDefinition.AccessModifiers = type.GetAccessModifiers().Select(x => x.ToString()).ToList();
        typeDefinition.Name = type.GetName();
        typeDefinition.Kind = type.GetKind().ToString();
        typeDefinition.IsGeneric = type.IsGenericType;
        typeDefinition.GenericType = !type.GetGenericArguments().Any() ? null : type.GetGenericArguments().Select(a => a.GetName()).ToList();
        typeDefinition.Interfaces = !type.GetInterfaces().Any() ? null : type.GetInterfaces().Select(i => i.GetName()).ToList();
        typeDefinition.InnerInterfaces = !type.GetAllInterfaces().Any() ? null : type.GetAllInterfaces().Select(i => i.GetName()).ToList();
        typeDefinition.BaseType = type.BaseType?.GetName();
        typeDefinition.BaseGenericType = type.BaseType?.GetGenericArguments().Select(a => a.GetName()).ToList();
        typeDefinition.GenericConstraints = genericConstraints.Any() ? genericConstraints : null;

        typeDefinition.PredefinedAttributes = type.GetPredefinedAttributes().ToList();

        var props = new List<MemberDefinition>();
        foreach (var prop in type.GetProperties())
        {
            var member = new MemberDefinition
            {
                AccessModifiers = !prop.PropertyType.GetAccessModifiers().Any() ? null : prop.PropertyType.GetAccessModifiers().Select(x => x.ToString()).ToList(),
                GenericTypes = !prop.PropertyType.GetGenericArguments().Any() ? null : prop.PropertyType.GetGenericArguments().Select(a => a.GetName()).ToList(),
                IsEnumerable = prop.PropertyType.IsEnumerable(),
                IsGeneric = prop.PropertyType.IsGenericType,
                IsNullable = prop.PropertyType.IsNullable(),
                TypeInterfaces = !prop.PropertyType.GetAllInterfaces().Any() ? null : prop.PropertyType.GetAllInterfaces().Select(i => i.GetName()).ToList(),
                IsEnumMember = false,
                Name = prop.Name,
                Type = prop.PropertyType.GetName(),
                EnumName = null,
                EnumValue = null,
            };
            props.Add(member);
        }
        typeDefinition.Members = props;
        return typeDefinition;
    }
}
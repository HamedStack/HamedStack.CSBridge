using HamedStack.CSBridge.Models;

namespace HamedStack.CSBridge.Helpers;

internal static class EnumTypeDefinitionHelper
{
    internal static TypeDefinition GetEnumTypeDefinition(this Type type)
    {
        var enumValues = Enum.GetValues(type);
        var typeDefinition = new TypeDefinition
        {
            AccessModifiers = type.GetAccessModifiers().Select(x => x.ToString()).ToList(),
            Name = type.GetName(),
            BaseType = type.GetEnumUnderlyingType().GetName(),
            Kind = type.GetKind().ToString()
        };
        var props = new List<MemberDefinition>();
        foreach (var val in enumValues)
        {
            var enumVal = (long)Convert.ChangeType(val, typeof(long));
            var member = new MemberDefinition
            {
                IsEnumMember = true,
                EnumName = val.ToString(),
                EnumValue = enumVal
            };
            props.Add(member);
        }
        typeDefinition.Members = props;

        typeDefinition.PredefinedAttributes = type.GetPredefinedAttributes().ToList();
        return typeDefinition;
    }
}
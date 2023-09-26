using System.Text.Encodings.Web;
using System.Text.Json;
using TypeDefinition = HamedStack.CSBridge.Models.TypeDefinition;

namespace HamedStack.CSBridge.Helpers;

public static class TypeDefinitionHelper
{
    public static TypeDefinition GetTypeDefinition(this Type type)
    {
        return type.IsEnum ? type.GetEnumTypeDefinition() : type.GetClassOrInterfaceOrStructTypeDefinition();
    }

    public static string GetTypeDefinitionJson(this Type type, JsonSerializerOptions? jsonSerializerOptions = default)
    {
        var options = jsonSerializerOptions ?? new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
        return JsonSerializer.Serialize(type.GetTypeDefinition(), options);
    }

    public static string GetTypeDefinitionJsonSchema(bool indented = false)
    {
        var jsonSchema = """
            {"$schema":"http://json-schema.org/draft-04/schema#","title":"TypeDefinition","type":"object","additionalProperties":false,"properties":{"AccessModifiers":{"type":"array","items":{"type":"string"}},"BaseGenericType":{"type":["array","null"],"items":{"type":"string"}},"BaseType":{"type":["null","string"]},"GenericConstraints":{"type":["array","null"],"items":{"$ref":"#/definitions/GenericConstraintDefinition"}},"GenericType":{"type":["array","null"],"items":{"type":"string"}},"InnerInterfaces":{"type":["array","null"],"items":{"type":"string"}},"Interfaces":{"type":["array","null"],"items":{"type":"string"}},"IsGeneric":{"type":"boolean"},"Kind":{"type":"string"},"Members":{"type":["array","null"],"items":{"$ref":"#/definitions/MemberDefinition"}},"Name":{"type":"string"},"PredefinedAttributes":{"type":["array","null"],"items":{"$ref":"#/definitions/AttributeDefinition"}}},"definitions":{"GenericConstraintDefinition":{"type":"object","additionalProperties":false,"properties":{"Members":{"type":"array","items":{"type":"string"}},"Name":{"type":"string"}}},"MemberDefinition":{"type":"object","additionalProperties":false,"properties":{"AccessModifiers":{"type":["array","null"],"items":{"type":"string"}},"EnumName":{"type":["null","string"]},"EnumValue":{"type":["integer","null"],"format":"int64"},"GenericTypes":{"type":["array","null"],"items":{"type":"string"}},"IsEnumerable":{"type":"boolean"},"IsEnumMember":{"type":"boolean"},"IsGeneric":{"type":"boolean"},"IsNullable":{"type":"boolean"},"Name":{"type":["null","string"]},"Type":{"type":["null","string"]},"TypeInterfaces":{"type":["array","null"],"items":{"type":"string"}}}},"AttributeDefinition":{"type":"object","additionalProperties":false,"properties":{"Arguments":{"type":["array","null"],"items":{"$ref":"#/definitions/AttributeArgumentDefinition"}},"Name":{"type":"string"},"Target":{"type":"string"},"TargetName":{"type":"string"}}},"AttributeArgumentDefinition":{"type":"object","additionalProperties":false,"properties":{"Name":{"type":"string"},"Values":{"type":"array","items":{"type":"string"}}}}}}
            """;

        using var jDoc = JsonDocument.Parse(jsonSchema);
        return JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = indented });
    }
}
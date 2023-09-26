using System.Collections;
using System.Text.RegularExpressions;
using HamedStack.CSBridge.Enums;

namespace HamedStack.CSBridge.Helpers;

internal static class TypescriptUtility
{
    internal static string FromCSharpStringType(string? csharpType, string[]? typeInterfaces, string[]? genericTypes, bool returnTypeIfUnknown = false)
    {
        if (csharpType == null)
        {
            throw new ArgumentNullException(nameof(csharpType));
        }
        typeInterfaces ??= Array.Empty<string>();
        genericTypes ??= Array.Empty<string>();
        csharpType = SimplifyGenerics(csharpType, true).RemoveWhitespace();
        var ti = typeInterfaces.Select(x => SimplifyGenerics(x, true).RemoveWhitespace()).Distinct().ToList();
        var final = new List<string> { csharpType };
        final.AddRange(ti);
        foreach (var item in final)
        {
            var mapper = GetMapper(false, genericTypes).FirstOrDefault(x => x.NameWithNoGeneric == item);
            if (mapper != null)
            {
                return mapper.TypeScriptType;
            }
        }

        if (!returnTypeIfUnknown) return "unknown";

        var result = csharpType;
        if (genericTypes.Length <= 0) return result;

        var generics = genericTypes.Select(x => FromCSharpStringType(x, null, null, true)).ToArray();
        result = $"{result}<{string.Join(", ", generics)}>";
        return result;
    }

    private static IEnumerable<MapperResult> GetMapper(bool useAny = false, params string[]? genericTypes)
    {
        var reservedTypes = Enumerable.Repeat(useAny ? "any" : "unknown", 8).ToArray();

        if (genericTypes is { Length: > 0 })
        {
            for (var i = 0; i < genericTypes.Length; i++)
            {
                reservedTypes[i] = genericTypes[i];
            }
        }

        var mapper = new Dictionary<Type, string>
        {
            {typeof(int), "number"},
            {typeof(uint), "number"},
            {typeof(long), "number"},
            {typeof(ulong), "number"},
            {typeof(short), "number"},
            {typeof(ushort), "number"},
            {typeof(float), "number"},
            {typeof(double), "number"},
            {typeof(decimal), "number"},
            {typeof(byte), "number"},
            {typeof(sbyte), "number"},
            {typeof(char), "string"},
            {typeof(string), "string"},
            {typeof(string[]), "Array<string>"},
            {typeof(bool), "boolean"},
            {typeof(DateTime), "string | Date"},
            {typeof(DateTimeOffset), "string | Date"},
            {typeof(DayOfWeek), "number | string"},
            {typeof(TimeSpan), "number | string"},
            {typeof(Guid), "string"},
            {typeof(Uri), "string"},
            {typeof(HashSet<>), $"Set<{reservedTypes[0]}>"},
            {typeof(SortedSet<>), $"Set<{reservedTypes[0]}>"},
            {typeof(IDictionary), $"Record<string, {reservedTypes[0]}>"},
            {typeof(IReadOnlyDictionary<,>), $"Record<{reservedTypes[0]}, {reservedTypes[1]}>"},
            {typeof(Stream), "Blob"},
            {typeof(Tuple<>), $"{{ item1: {reservedTypes[0]}; }}"},
            {typeof(Tuple<,>), $"{{ item1: {reservedTypes[0]}; item2: {reservedTypes[1]}; }}"},
            {typeof(Tuple<,,>), $"{{ item1: {reservedTypes[0]}; item2: {reservedTypes[1]}; item3: {reservedTypes[2]}; }}"},
            {typeof(Tuple<,,,>), $"{{ item1: {reservedTypes[0]}; item2: {reservedTypes[1]}; item3: {reservedTypes[2]}; item4: {reservedTypes[3]}; }}"},
            {typeof(Tuple<,,,,>), $"{{ item1: {reservedTypes[0]}; item2: {reservedTypes[1]}; item3: {reservedTypes[2]}; item4: {reservedTypes[3]}; item5: {reservedTypes[4]}; }}"},
            {typeof(Tuple<,,,,,>), $"{{ item1: {reservedTypes[0]}; item2: {reservedTypes[1]}; item3: {reservedTypes[2]}; item4: {reservedTypes[3]}; item5: {reservedTypes[4]}; item6: {reservedTypes[5]}; }}"},
            {typeof(Tuple<,,,,,,>),$"{{ item1: {reservedTypes[0]}; item2: {reservedTypes[1]}; item3: {reservedTypes[2]}; item4: {reservedTypes[3]}; item5: {reservedTypes[4]}; item6: {reservedTypes[5]}; item7: {reservedTypes[7]}; }}"},
            {typeof(ValueTuple<>), $"{{ item1: {reservedTypes[0]}; }}"},
            {typeof(ValueTuple<,>), $"{{ item1: {reservedTypes[0]}; item2: {reservedTypes[1]}; }}"},
            {typeof(ValueTuple<,,>), $"{{ item1: {reservedTypes[0]}; item2: {reservedTypes[1]}; item3: {reservedTypes[2]}; }}"},
            {typeof(ValueTuple<,,,>), $"{{ item1: {reservedTypes[0]}; item2: {reservedTypes[1]}; item3: {reservedTypes[2]}; item4: {reservedTypes[3]}; }}"},
            {typeof(ValueTuple<,,,,>), $"{{ item1: {reservedTypes[0]}; item2: {reservedTypes[1]}; item3: {reservedTypes[2]}; item4: {reservedTypes[3]}; item5: {reservedTypes[4]}; }}"},
            {typeof(ValueTuple<,,,,,>), $"{{ item1: {reservedTypes[0]}; item2: {reservedTypes[1]}; item3: {reservedTypes[2]}; item4: {reservedTypes[3]}; item5: {reservedTypes[4]}; item6: {reservedTypes[5]}; }}"},
            {typeof(ValueTuple<,,,,,,>),$"{{ item1: {reservedTypes[0]}; item2: {reservedTypes[1]}; item3: {reservedTypes[2]}; item4: {reservedTypes[3]}; item5: {reservedTypes[4]}; item6: {reservedTypes[5]}; item7: {reservedTypes[7]}; }}"},
            {typeof(IEnumerable), $"Array<{reservedTypes[0]}>"},
            {typeof(object), reservedTypes[0]},
        };

        var result = mapper.Select(item => new MapperResult
        {
            Type = item.Key,
            Name = item.Key.GetName(),
            FullName = item.Key.GetName(fullName: true),
            SimplifiedGeneric = item.Key.GetName(GenericPresentationMode.Simplified),
            NameWithNoGeneric = item.Key.GetName(GenericPresentationMode.Cleaned),
            TypeScriptType = item.Value,
            IsGeneric = item.Key.GetName().Contains('<') || item.Value.Contains('<')
        })
            .ToList();

        return result;
    }

    private static string SimplifyGenerics(string text, bool removeGenericPart = false)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(text));

        if (!text.Contains('<'))
        {
            return text;
        }
        if (removeGenericPart)
        {
            text = Regex.Replace(text, "<.+>", "", RegexOptions.Compiled);
            return text.RemoveWhitespace();
        }

        if (!text.Contains(","))
        {
            text = Regex.Replace(text, "<.+>", "<>", RegexOptions.Compiled);
        }
        else
        {
            while (true)
            {
                var isMatch1 = Regex.IsMatch(text, "<[^<>,]+?,", RegexOptions.Compiled);
                var isMatch2 = Regex.IsMatch(text, ",[^<>,]+>", RegexOptions.Compiled);
                var isMatch3 = Regex.IsMatch(text, ",[^<>,]+?,", RegexOptions.Compiled);

                if (!isMatch1 && !isMatch2 && !isMatch3) break;

                text = Regex.Replace(text, "<[^<>,]+?,", "<,", RegexOptions.Compiled);
                text = Regex.Replace(text, ",[^<>,]+>", ",>", RegexOptions.Compiled);
                text = Regex.Replace(text, ",[^<>,]+?,", ",,", RegexOptions.Compiled);
            }
        }

        return text.RemoveWhitespace();
    }

    private class MapperResult
    {
        public string FullName { get; init; } = null!;
        public bool IsGeneric { get; set; }
        public string Name { get; init; } = null!;
        public string NameWithNoGeneric { get; init; } = null!;
        public string SimplifiedGeneric { get; init; } = null!;
        public Type Type { get; init; } = null!;
        public string TypeScriptType { get; init; } = null!;
    }
}
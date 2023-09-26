using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using HamedStack.CSBridge.Enums;
using Humanizer;

namespace HamedStack.CSBridge.Helpers;

internal static class Extensions
{
    internal static string ConvertTextTo(this string text, HumanizerInflectorType inflectorType)
    {
        if (text == null) throw new ArgumentNullException(nameof(text));
        if (string.IsNullOrWhiteSpace(text))
            return text;

        return inflectorType switch
        {
            HumanizerInflectorType.Pluralize => text.Pluralize(false),
            HumanizerInflectorType.Singularize => text.Singularize(false),
            HumanizerInflectorType.Titleize => text.Titleize(),
            HumanizerInflectorType.Pascalize => text.Pascalize(),
            HumanizerInflectorType.Camelize => text.Camelize(),
            HumanizerInflectorType.Underscore => text.Underscore(),
            HumanizerInflectorType.Dasherize => text.Dasherize(),
            _ => throw new ArgumentOutOfRangeException(nameof(inflectorType), inflectorType, null)
        };
    }

    internal static IEnumerable<TU> FindDuplicates<T, TU>(this IEnumerable<T> list, Func<T, TU> keySelector)
    {
        return list.GroupBy(keySelector)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key).ToList();
    }

    internal static IEnumerable<AccessModifier> GetAccessModifiers(this Type type)
    {
        var accessModifiers = new List<AccessModifier>();

        if (type.IsPublic)
        {
            accessModifiers.Add(AccessModifier.Public);
        }
        else if (type.IsNotPublic)
        {
            accessModifiers.Add(AccessModifier.Private);
        }
        else if (type.IsNestedFamily)
        {
            accessModifiers.Add(AccessModifier.Protected);
        }
        else if (type.IsNestedAssembly)
        {
            accessModifiers.Add(AccessModifier.Internal);
        }

        return accessModifiers;
    }

    internal static IEnumerable<Type> GetAllInterfaces(this Type type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        var interfaces = type.GetInterfaces();

        var baseType = type.BaseType;
        while (baseType != null)
        {
            interfaces = interfaces.Concat(baseType.GetInterfaces()).ToArray();
            baseType = baseType.BaseType;
        }

        interfaces = interfaces.SelectMany(i => i.GetAllInterfaces()).Concat(interfaces).ToArray();

        return interfaces.Distinct();
    }

    internal static Dictionary<string, List<string>> GetGenericConstraints(this Type type)
    {
        var result = new Dictionary<string, List<string>>();

        foreach (var genericArgument in type.GetGenericArguments())
        {
            var gpa = genericArgument.GenericParameterAttributes;
            var constraints = genericArgument.GetGenericParameterConstraints().Select(constraintType => constraintType.Name).ToList();

            if ((gpa & GenericParameterAttributes.DefaultConstructorConstraint) != 0)
            {
                constraints.Add("new()");
            }

            if ((gpa & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0)
            {
                constraints.Add("notnull");
            }

            result.Add(genericArgument.Name, constraints);
        }

        return result;
    }

    internal static Kind GetKind(this Type type)
    {
        if (type.IsClass)
        {
            return Kind.Class;
        }

        if (type.IsInterface)
        {
            return Kind.Interface;
        }
        if (type.IsEnum)
        {
            return Kind.Enum;
        }
        if (type.IsValueType)
        {
            return Kind.ValueType;
        }
        throw new ArgumentException("Invalid type");
    }

    internal static string GetName(this Type typeRef, GenericPresentationMode genericMode = GenericPresentationMode.Normal, bool fullName = false)
    {
        var name = GetPrettyName(typeRef, fullName);

        return genericMode switch
        {
            GenericPresentationMode.Cleaned => Regex.Replace(name, "<.+>", "", RegexOptions.Compiled),
            GenericPresentationMode.Simplified => SimplifyGenerics(name),
            GenericPresentationMode.Normal => name,
            _ => throw new ArgumentOutOfRangeException(nameof(genericMode), genericMode, null)
        };

        static string GetPrettyName(Type typeRef, bool fullName = false)
        {
            var rootType = typeRef.IsGenericType
                ? typeRef.GetGenericTypeDefinition()
                : typeRef;

            var cleanedName = rootType.IsPrimitive
                ? rootType.Name
                : rootType.ToString();

            if (!fullName && typeRef.Namespace != null && cleanedName.StartsWith(typeRef.Namespace))
                cleanedName = cleanedName.Substring(typeRef.Namespace.Length + 1);

            if (!typeRef.IsGenericType)
                return cleanedName;
            return cleanedName
                       .Substring(0, cleanedName.LastIndexOf('`'))
                   + typeRef.GetGenericArguments()
                       .Aggregate("<", (r, i) => r + (r != "<" ? ", " : null) + GetPrettyName(i, fullName)) + ">";
        }
        static string SimplifyGenerics(string text)
        {
            if (!text.Contains('<'))
            {
                return text;
            }
            if (!text.Contains(','))
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

            return RemoveAllWhitespaces(text);
        }

        static string RemoveAllWhitespaces(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }
    }

    internal static bool IsEnumerable(this Type type, bool excludeString = true)
    {
        if (excludeString && type == typeof(string))
        {
            return false;
        }
        return type.GetInterfaces().Any(x => x == typeof(IEnumerable));
    }

    internal static bool IsNullable(this Type type)
    {
        return Nullable.GetUnderlyingType(type) != null;
    }

    internal static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
    {
        return source == null || !source.Any();
    }

    internal static bool IsReadOnly(this PropertyInfo prop)
    {
        if (!prop.CanWrite)
        {
            return true;
        }

        var setMethod = prop.GetSetMethod(true);

        return setMethod == null || setMethod.IsPrivate;
    }

    internal static bool IsStatic(this Type type)
    {
        return type is { IsAbstract: true, IsSealed: true };
    }

    internal static string RemoveWhitespace(this string input)
    {
        return new string(input.ToCharArray()
            .Where(c => !char.IsWhiteSpace(c))
            .ToArray());
    }
}
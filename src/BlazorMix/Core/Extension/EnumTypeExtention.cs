
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BlazorMix;

public static class EnumTypeExtention
{
    public static T? GetEnumByName<T>(this string name)
    {
        foreach (var memberInfo in typeof(T).GetMembers())
        {
            foreach (var attr in memberInfo.GetCustomAttributes(true))
            {
                var test = attr as DisplayAttribute;

                if (test == null) continue;

                if (test.Name == name)
                {
                    var result = (T)Enum.Parse(typeof(T), memberInfo.Name);

                    return result;
                }
            }
        }

        return default;
    }

    public static TAttribute? GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
    {
        return enumValue.GetType()
                        ?.GetMember(enumValue.ToString())
                        ?.First()
                        ?.GetCustomAttribute<TAttribute>();
    }

    public static string GetDisplayName(this Enum @enum)
    {
        return @enum.GetAttribute<DisplayAttribute>()?.Name ?? @enum.ToString();
    }
}

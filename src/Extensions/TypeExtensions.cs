using System;

namespace WHampson.ToolUI.Extensions
{
    internal static class TypeExtensions
    {
        public static Type GetUnderlyingType(this Type t)
        {
            if (t == null)
            {
                return null;
            }

            t = t.GetElementType() ?? t;
            t = (t.IsEnum) ? t.GetEnumUnderlyingType() : t;
            t = Nullable.GetUnderlyingType(t) ?? t;

            return t;
        }

        public static bool IsNumeric(this Type t)
        {
            if (t == null) return false;
            if (t == typeof(decimal)) return true;

            return t.IsPrimitive && t != typeof(bool) && t != typeof(char);
        }
    }
}

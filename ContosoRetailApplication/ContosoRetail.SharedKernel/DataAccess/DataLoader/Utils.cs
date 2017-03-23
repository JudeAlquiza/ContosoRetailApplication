using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ContosoRetail.SharedKernel.DataAccess.DataLoader
{
    public static class Utils
    {

        public static bool CanAssignNull(Type type)
        {
            return !type.GetTypeInfo().IsValueType || IsNullable(type);
        }

        public static bool IsNullable(Type type)
        {
            var info = type.GetTypeInfo();
            return info.IsGenericType && info.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static object GetDefaultValue(Type type)
        {
            if (type.GetTypeInfo().IsValueType)
                return Activator.CreateInstance(type);
            return null;
        }

        public static object ConvertClientValue(object value, Type type)
        {
            if (value == null || type == null)
                return value;

            type = StripNullableType(type);

            if (IsIntegralType(type) && value is String)
                value = Convert.ToDecimal(value, CultureInfo.InvariantCulture);

            if (type == typeof(DateTime) && value is String)
                return DateTime.Parse((string)value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

            var converter = TypeDescriptor.GetConverter(type);
            if (converter != null && converter.CanConvertFrom(value.GetType()))
                return converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);

            if (type.GetTypeInfo().IsEnum)
                return Enum.Parse(type, Convert.ToString(value), true);

            return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
        }

        public static Type StripNullableType(Type type)
        {
            var underlying = Nullable.GetUnderlyingType(type);
            if (underlying != null)
                return underlying;

            return type;
        }

        public static string GetSortMethod(bool first, bool desc)
        {
            return first
                ? (desc ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy))
                : (desc ? nameof(Queryable.ThenByDescending) : nameof(Queryable.ThenBy));
        }

        public static IEnumerable<ContosoRetail.SharedKernel.DataAccess.DataLoader.SortingInfo> AddRequiredSort(IEnumerable<ContosoRetail.SharedKernel.DataAccess.DataLoader.SortingInfo> sort, IEnumerable<string> requiredSelectors)
        {
            sort = sort ?? new ContosoRetail.SharedKernel.DataAccess.DataLoader.SortingInfo[0];
            requiredSelectors = requiredSelectors.Except(sort.Select(i => i.Selector));

            var desc = sort.LastOrDefault()?.Desc;

            return sort.Concat(requiredSelectors.Select(i => new ContosoRetail.SharedKernel.DataAccess.DataLoader.SortingInfo
            {
                Selector = i,
                Desc = desc != null && desc.Value
            }));
        }

        public static string[] GetPrimaryKey(Type type)
        {
            return new MemberInfo[0]
                .Concat(type.GetRuntimeProperties())
                .Concat(type.GetRuntimeFields())
                .Where(m => m.GetCustomAttributes(true).Any(i => i.GetType().Name == "KeyAttribute"))
                .Select(m => m.Name)
                .OrderBy(i => i)
                .ToArray();
        }

        static bool IsIntegralType(Type type)
        {
            return type == typeof(int)
                || type == typeof(long)
                || type == typeof(byte)
                || type == typeof(sbyte)
                || type == typeof(uint)
                || type == typeof(ulong);
        }

    }
}

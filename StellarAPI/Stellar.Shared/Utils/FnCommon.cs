using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace Stellar.Shared.Utils
{
    public static class FnCommon
    {
        public static void CopyProperties(object target, object source)
        {
            if (target == null || source == null) return;

            var sourceProps = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var targetProps = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var sProp in sourceProps)
            {
                if (!sProp.CanRead) continue;

                var tProp = targetProps.FirstOrDefault(p => p.Name == sProp.Name && p.CanWrite);
                if (tProp != null)
                {
                    // Basic type check or assignment compat
                    if (tProp.PropertyType.IsAssignableFrom(sProp.PropertyType))
                    {
                        var val = sProp.GetValue(source);
                        tProp.SetValue(target, val);
                    }
                }
            }
        }

        public static void CopyNotNullProperties(object target, object source)
        {
            if (target == null || source == null) return;

            var nullPropertyNames = GetNullPropertyNames(source);
            var sourceProps = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var targetProps = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var sProp in sourceProps)
            {
                if (!sProp.CanRead) continue;
                if (nullPropertyNames.Contains(sProp.Name)) continue;

                var tProp = targetProps.FirstOrDefault(p => p.Name == sProp.Name && p.CanWrite);
                if (tProp != null)
                {
                    if (tProp.PropertyType.IsAssignableFrom(sProp.PropertyType))
                    {
                        var val = sProp.GetValue(source);
                        tProp.SetValue(target, val);
                    }
                }
            }
        }

        public static T CopyNonNullProperties<T>(Type type, object source)
        {
            var target = Activator.CreateInstance(type);
            if (target == null) return default;
            CopyNotNullProperties(target, source);
            return (T)target;
        }

        public static T CopyProperties<T>(Type type, object source)
        {
             return CopyNonNullProperties<T>(type, source);
        }

        public static string[] GetNullPropertyNames(object source)
        {
            if (source == null) return Array.Empty<string>();

            var properties = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var nullProps = new List<string>();

            foreach (var prop in properties)
            {
                if (prop.CanRead && prop.GetValue(source) == null)
                {
                    nullProps.Add(prop.Name);
                }
            }

            return nullProps.ToArray();
        }

        public static bool NotNullOrBlank(string? str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }
    }
}

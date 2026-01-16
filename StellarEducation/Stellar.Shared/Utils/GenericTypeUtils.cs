using System;
using System.Reflection;
using System.Linq;
using Stellar.Shared.Models.Enums;

namespace Stellar.Shared.Utils
{
    public static class GenericTypeUtils
    {
        /// <summary>
        /// Get the value of a field via reflection.
        /// </summary>
        /// <typeparam name="T">Target object type</typeparam>
        /// <param name="target">Target object</param>
        /// <param name="fieldName">Field name</param>
        /// <returns>Field value</returns>
        public static object? GetFieldValue<T>(T target, string fieldName)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var field = target.GetType().GetField(fieldName, 
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (field == null)
            {
                // Try property instead
                var property = target.GetType().GetProperty(fieldName, 
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                
                if (property == null)
                    throw new InvalidOperationException(
                        $"Could not find field or property '{fieldName}' in class {target.GetType().Name}");
                
                return property.GetValue(target);
            }

            return field.GetValue(target);
        }

        /// <summary>
        /// Set the value of a field via reflection.
        /// </summary>
        /// <typeparam name="T">Target object type</typeparam>
        /// <param name="target">Target object</param>
        /// <param name="fieldName">Field name</param>
        /// <param name="value">Value to set</param>
        public static void UpdateData<T>(T target, string fieldName, object? value)
        {
            if (target == null) return;

            try
            {
                var field = target.GetType().GetField(fieldName, 
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                
                if (field != null)
                {
                    field.SetValue(target, value);
                    return;
                }

                var property = target.GetType().GetProperty(fieldName, 
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                
                if (property != null && property.CanWrite)
                {
                    property.SetValue(target, value);
                }
            }
            catch
            {
                // Silently ignore errors as per Java implementation
            }
        }

        /// <summary>
        /// Create a new instance of a generic type parameter.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <typeparam name="S">Source class type</typeparam>
        /// <param name="superClass">Instance of the source class</param>
        /// <param name="interfaceType">Interface or base class to search for</param>
        /// <param name="positionType">Position of the generic parameter</param>
        /// <returns>New instance of the generic type</returns>
        public static T GetNewInstance<T, S>(S superClass, Type interfaceType, PositionType positionType)
        {
            if (superClass == null)
                throw new ArgumentNullException(nameof(superClass));

            var currentType = superClass.GetType();
            var targetGenericType = FindGenericTypeInHierarchy(currentType, interfaceType);

            if (targetGenericType != null && targetGenericType.IsGenericType)
            {
                var typeArgs = targetGenericType.GetGenericArguments();
                
                int index = positionType == PositionType.LAST 
                    ? typeArgs.Length - 1 
                    : (int)positionType.GetValue();

                if (index < typeArgs.Length)
                {
                    var targetType = typeArgs[index];
                    
                    if (!targetType.IsGenericParameter)
                    {
                        var constructor = targetType.GetConstructor(
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, 
                            null, Type.EmptyTypes, null);
                        
                        if (constructor != null)
                        {
                            return (T)constructor.Invoke(null);
                        }
                    }
                }
            }

            throw new ArgumentException(
                $"Cannot determine generic type at position {positionType} for interface {interfaceType.Name}");
        }

        private static Type? FindGenericTypeInHierarchy(Type startType, Type targetInterface)
        {
            var currentType = startType;

            while (currentType != null)
            {
                // Check generic base class
                if (currentType.BaseType != null && currentType.BaseType.IsGenericType)
                {
                    var baseType = currentType.BaseType;
                    if (IsAssignableFrom(targetInterface, baseType.GetGenericTypeDefinition()))
                    {
                        return baseType;
                    }
                }

                // Check interfaces
                var foundType = SearchInInterfaces(currentType.GetInterfaces(), targetInterface);
                if (foundType != null)
                {
                    return foundType;
                }

                currentType = currentType.BaseType;
            }

            return null;
        }

        private static Type? SearchInInterfaces(Type[] interfaces, Type targetInterface)
        {
            foreach (var interfaceType in interfaces)
            {
                if (interfaceType.IsGenericType)
                {
                    var genericDef = interfaceType.GetGenericTypeDefinition();
                    
                    if (IsAssignableFrom(targetInterface, genericDef))
                    {
                        return interfaceType;
                    }

                    var foundInParent = SearchInInterfaces(genericDef.GetInterfaces(), targetInterface);
                    if (foundInParent != null)
                    {
                        return foundInParent;
                    }
                }
                else
                {
                    if (IsAssignableFrom(targetInterface, interfaceType))
                    {
                        return interfaceType;
                    }

                    var foundInParent = SearchInInterfaces(interfaceType.GetInterfaces(), targetInterface);
                    if (foundInParent != null)
                    {
                        return foundInParent;
                    }
                }
            }

            return null;
        }

        private static bool IsAssignableFrom(Type target, Type candidate)
        {
            return target.IsAssignableFrom(candidate) || candidate.IsAssignableFrom(target);
        }
    }
}

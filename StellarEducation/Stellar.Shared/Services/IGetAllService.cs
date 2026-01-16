using Stellar.Shared.Interfaces;
using Stellar.Shared.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stellar.Shared.Models;
using Stellar.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace Stellar.Shared.Services
{
    public interface IGetAllService<E, RES> :
    IGetAllPersistenceProvider<E>,
    IResponseMapper<E, RES>
    {
        public Page<RES> GetAll(
            HeaderContext context,
            string? search,
            int page,
            int pageSize,
            string? sort,
            Dictionary<string, object> filter)
        {
            return GetAll(context, search, page, pageSize, sort, filter, MappingPageResponse);
        }

        public Page<RES> GetAll(
             HeaderContext context,
             string? search,
             int page,
             int pageSize,
             string? sort,
             Dictionary<string, object> filter,
             Func<HeaderContext, Page<E>, Page<RES>> mappingPageResponseHandler)
        {
            var query = GetGetAllPersistence().Query();

            query = BuildEntityQuery(query, context, filter);
            query = BuildFilterQuery(query, context, filter);
            query = BuildExpression(query, context, filter);
            query = BuildSearchQuery(query, context, search);
            query = ApplySort(query, sort);

            var totalElements = query.Count();

            var contentEntities = query
                .Skip(Math.Max(0, page - 1) * pageSize)
                .Take(pageSize)
                .ToList(); // Execute query

            var pageE = new Page<E>(contentEntities, page - 1, pageSize, totalElements);

            return mappingPageResponseHandler(context, pageE);
        }

        public Page<RES> MappingPageResponse(HeaderContext context, Page<E> items)
        {
            return items.Map(item =>
            {
                RES resItem = Activator.CreateInstance<RES>();
                FnCommon.CopyProperties(resItem, item);
                return resItem;
            });
        }

        IQueryable<E> BuildEntityQuery(IQueryable<E> query, HeaderContext context, Dictionary<string, object> filter)
        {
            if (filter == null || filter.Count == 0) return query;

            var parameter = Expression.Parameter(typeof(E), "x");
            Expression? finalExpression = null;
            var properties = typeof(E).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            // Supported Time Types: DateTime, DateTime?, DateTimeOffset, etc.
            var timeTypes = new HashSet<Type> 
            { 
                typeof(DateTime), typeof(DateTime?), 
                typeof(DateTimeOffset), typeof(DateTimeOffset?),
                typeof(TimeSpan), typeof(TimeSpan?)
            };

            foreach (var property in properties)
            {
                var fieldName = property.Name;
                var propertyType = property.PropertyType;
                var propertyAccess = Expression.Property(parameter, property);

                if (timeTypes.Contains(propertyType))
                {
                    // Date Range Logic: from.{Field} and to.{Field}
                    // Java ParamsKeys.getFieldName: prefix + "." + Capitalize(field)
                    // Ensure capitalization match
                    var capField = char.ToUpper(fieldName[0]) + fieldName.Substring(1);
                    var fromKey = $"from.{capField}";
                    var toKey = $"to.{capField}";
                    
                    // Check From
                    if (filter.ContainsKey(fromKey) || filter.ContainsKey(fromKey.ToLower()))
                    {
                        var val = filter.ContainsKey(fromKey) ? filter[fromKey] : filter[fromKey.ToLower()];
                         try {
                            var constant = CreateConstant(val, propertyType);
                            if (constant != null)
                            {
                                var greaterEqual = Expression.GreaterThanOrEqual(propertyAccess, constant);
                                finalExpression = finalExpression == null ? greaterEqual : Expression.AndAlso(finalExpression, greaterEqual);
                            }
                        } catch {}
                    }

                    // Check To
                    if (filter.ContainsKey(toKey) || filter.ContainsKey(toKey.ToLower()))
                    {
                        var val = filter.ContainsKey(toKey) ? filter[toKey] : filter[toKey.ToLower()];
                        try {
                            var constant = CreateConstant(val, propertyType);
                            if (constant != null)
                            {
                                var lessEqual = Expression.LessThanOrEqual(propertyAccess, constant);
                                finalExpression = finalExpression == null ? lessEqual : Expression.AndAlso(finalExpression, lessEqual);
                            }
                        } catch {}
                    }
                }
                else
                {
                    // Equality Logic
                    // Check exact or lower case key
                    string? matchKey = null;
                    if (filter.ContainsKey(fieldName)) matchKey = fieldName;
                    else if (filter.ContainsKey(fieldName.ToLower())) matchKey = fieldName.ToLower();
                    
                    // Java logic: skips if value is null
                    if (matchKey != null)
                    {
                        var targetValue = filter[matchKey];
                        if (targetValue != null) 
                        {
                             try 
                            {
                                var constant = CreateConstant(targetValue, propertyType);
                                if (constant != null)
                                {
                                    var equality = Expression.Equal(propertyAccess, constant);
                                    finalExpression = finalExpression == null ? equality : Expression.AndAlso(finalExpression, equality);
                                }
                            }
                            catch { }
                        }
                    }
                }
            }

            if (finalExpression != null)
            {
                var lambda = Expression.Lambda<Func<E, bool>>(finalExpression, parameter);
                return query.Where(lambda);
            }

            return query;
        }

        IQueryable<E> BuildExpression(IQueryable<E> query, HeaderContext context, Dictionary<string, object> filter)
        {
            if (filter == null || !filter.ContainsKey("expression")) return query;
            
            var expressionData = filter["expression"];
            var parameter = Expression.Parameter(typeof(E), "x");
            
            Expression? body = null;

            try
            {
                if (expressionData is JsonElement element)
                {
                    body = ParseJsonExpression(element, parameter);
                }
                else if (expressionData is string jsonString)
                {
                     var doc = JsonDocument.Parse(jsonString);
                     body = ParseJsonExpression(doc.RootElement, parameter);
                }
            }
            catch { }

            if (body != null)
            {
                var lambda = Expression.Lambda<Func<E, bool>>(body, parameter);
                return query.Where(lambda);
            }

            return query;
        }

        Expression? ParseJsonExpression(JsonElement element, ParameterExpression parameter)
        {
            if (element.ValueKind == JsonValueKind.Array)
            {
                // OR Logic
                Expression? orExpr = null;
                foreach (var item in element.EnumerateArray())
                {
                    var child = ParseJsonExpression(item, parameter);
                    if (child != null)
                    {
                        orExpr = orExpr == null ? child : Expression.OrElse(orExpr, child);
                    }
                }
                return orExpr;
            }
            else if (element.ValueKind == JsonValueKind.Object)
            {
                // AND Logic
                Expression? andExpr = null;
                foreach (var property in element.EnumerateObject())
                {
                    var fieldName = property.Name;
                    var valueResult = ParseCondition(fieldName, property.Value, parameter);
                    if (valueResult != null)
                    {
                        andExpr = andExpr == null ? valueResult : Expression.AndAlso(andExpr, valueResult);
                    }
                }
                return andExpr;
            }
            return null;
        }

        Expression? ParseCondition(string fieldName, JsonElement valueElement, ParameterExpression parameter)
        {
            var property = typeof(E).GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null) return null;

            var propertyAccess = Expression.Property(parameter, property);
            var propertyType = property.PropertyType;

            // Check if value is object with operators
            if (valueElement.ValueKind == JsonValueKind.Object)
            {
                Expression? opExpr = null;
                foreach (var opProp in valueElement.EnumerateObject())
                {
                    var op = opProp.Name.ToLower();
                    var val = opProp.Value;
                    Expression? currentOp = null;

                    try
                    {
                        if (op == "in" && val.ValueKind == JsonValueKind.Array)
                        {
                            // IN operator
                            var listType = typeof(List<>).MakeGenericType(propertyType);
                            var list = Activator.CreateInstance(listType);
                            var addMethod = listType.GetMethod("Add");

                            foreach (var item in val.EnumerateArray())
                            {
                                var constVal = ConvertJsonElement(item, propertyType);
                                if (constVal != null) addMethod?.Invoke(list, new[] { constVal });
                            }
                            
                            var containsMethod = listType.GetMethod("Contains");
                            var listConst = Expression.Constant(list, listType);
                            currentOp = Expression.Call(listConst, containsMethod, propertyAccess);
                        }
                        else
                        {
                            var constExpr = CreateConstant(val, propertyType);
                            if (constExpr != null)
                            {
                                switch (op)
                                {
                                    case ">": currentOp = Expression.GreaterThan(propertyAccess, constExpr); break;
                                    case ">=": currentOp = Expression.GreaterThanOrEqual(propertyAccess, constExpr); break;
                                    case "<": currentOp = Expression.LessThan(propertyAccess, constExpr); break;
                                    case "<=": currentOp = Expression.LessThanOrEqual(propertyAccess, constExpr); break;
                                    case "=": currentOp = Expression.Equal(propertyAccess, constExpr); break;
                                    case "!=": currentOp = Expression.NotEqual(propertyAccess, constExpr); break;
                                }
                            }
                        }
                    }
                    catch { }

                    if (currentOp != null)
                    {
                        opExpr = opExpr == null ? currentOp : Expression.AndAlso(opExpr, currentOp);
                    }
                }
                return opExpr;
            }
            else
            {
                // value is direct value -> Equality
                var constExpr = CreateConstant(valueElement, propertyType);
                return constExpr != null ? Expression.Equal(propertyAccess, constExpr) : null;
            }
        }

        ConstantExpression? CreateConstant(object value, Type targetType)
        {
            if (value is JsonElement je) return Expression.Constant(ConvertJsonElement(je, targetType), targetType);
            return Expression.Constant(Convert.ChangeType(value, Nullable.GetUnderlyingType(targetType) ?? targetType), targetType);
        }

        object? ConvertJsonElement(JsonElement jsonElement, Type propertyType)
        {
             object? convertedValue = null;
             try {
                if (propertyType == typeof(string)) convertedValue = jsonElement.ToString();
                else if (propertyType == typeof(int) || propertyType == typeof(int?)) convertedValue = jsonElement.GetInt32();
                else if (propertyType == typeof(long) || propertyType == typeof(long?)) convertedValue = jsonElement.GetInt64();
                else if (propertyType == typeof(bool) || propertyType == typeof(bool?)) convertedValue = jsonElement.GetBoolean();
                else if (propertyType == typeof(double) || propertyType == typeof(double?)) convertedValue = jsonElement.GetDouble();
                else if (propertyType == typeof(Guid) || propertyType == typeof(Guid?)) convertedValue = Guid.Parse(jsonElement.ToString());
                else if (propertyType.IsEnum) convertedValue = Enum.Parse(propertyType, jsonElement.ToString(), true);
                else convertedValue = JsonSerializer.Deserialize(jsonElement.GetRawText(), propertyType);
             } catch {}
             return convertedValue;
        }

        // Hook for custom filter logic - override in concrete service
        IQueryable<E> BuildFilterQuery(IQueryable<E> query, HeaderContext context, Dictionary<string, object> filter)
        {
            return query;
        }

        // Basic search implementation
        IQueryable<E> BuildSearchQuery(IQueryable<E> query, HeaderContext context, string? search)
        {
            if (string.IsNullOrWhiteSpace(search)) return query;
            
            var searchFields = GetSearchFieldNames();
            var parameter = Expression.Parameter(typeof(E), "x");
            Expression? finalExpression = null;
            
            var searchLower = Expression.Constant(search.ToLower());
            var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            foreach (var fieldName in searchFields)
            {
                var property = typeof(E).GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property != null && property.PropertyType == typeof(string))
                {
                    // x.Property
                    var propertyAccess = Expression.Property(parameter, property);
                    
                    // x.Property.ToLower()
                    // Note: Check for null if necessary? EF Core handles null propagation usually as boolean false for Contains.
                    // But standard C# would throw. 
                    // EF handles: (x.Property != null && x.Property.ToLower().Contains(...))
                    // Let's rely on EF Core translation or add Null check.
                    
                    var nullCheck = Expression.NotEqual(propertyAccess, Expression.Constant(null));
                    
                    var toLowerCall = Expression.Call(propertyAccess, toLowerMethod);
                    var containsCall = Expression.Call(toLowerCall, containsMethod, searchLower);
                    
                    var combined = Expression.AndAlso(nullCheck, containsCall);

                    finalExpression = finalExpression == null ? combined : Expression.OrElse(finalExpression, combined);
                }
            }

            if (finalExpression != null)
            {
                 var lambda = Expression.Lambda<Func<E, bool>>(finalExpression, parameter);
                 return query.Where(lambda);
            }
            
            return query;
        }

        string[] GetSearchFieldNames()
        {
            return new string[] { "name", "code", "ten", "ma" };
        }

        // Sort logic matching Java: {"field": 1/-1} or {"field": "1"/"-1"}
        IQueryable<E> ApplySort(IQueryable<E> query, string? sort)
        {
            if (string.IsNullOrWhiteSpace(sort)) return query;

            try
            {
                // Deserialize to object to handle both 1 (number) and "1" (string)
                var sortMap = JsonSerializer.Deserialize<Dictionary<string, object>>(sort);
                if (sortMap == null || sortMap.Count == 0) return query;

                bool first = true;

                foreach (var kvp in sortMap)
                {
                    var propertyName = kvp.Key;
                    var valObj = kvp.Value;
                    int direction = 1; // Default ASC

                    // Parse direction safely matching Java NumberUtils behavior
                    if (valObj is JsonElement je)
                    {
                        if (je.ValueKind == JsonValueKind.Number) direction = je.GetInt32();
                        else if (je.ValueKind == JsonValueKind.String && int.TryParse(je.GetString(), out int d)) direction = d;
                    }
                    else if (valObj != null)
                    {
                        try { direction = Convert.ToInt32(valObj); } catch { }
                    }

                    var property = typeof(E).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property == null) continue;

                    var parameter = Expression.Parameter(typeof(E), "x");
                    var propertyAccess = Expression.Property(parameter, property);
                    var keySelector = Expression.Lambda(propertyAccess, parameter);

                    string methodName = "";
                    if (first)
                    {
                        methodName = direction == -1 ? "OrderByDescending" : "OrderBy";
                    }
                    else
                    {
                        methodName = direction == -1 ? "ThenByDescending" : "ThenBy";
                    }

                    var resultExpression = Expression.Call(
                        typeof(Queryable),
                        methodName,
                        new Type[] { typeof(E), property.PropertyType },
                        query.Expression,
                        Expression.Quote(keySelector));

                    query = query.Provider.CreateQuery<E>(resultExpression);
                    first = false;
                }
            }
            catch
            {
                // Ignore parse errors
            }
            
            return query;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Stellar.Shared.Utils
{
    public static class JsonParserUtils
    {
        private static JsonSerializerOptions? _options;

        public static JsonSerializerOptions GetJsonOptions()
        {
            if (_options == null)
            {
                _options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    Converters = { new JsonStringEnumConverter() }
                };
            }

            return _options;
        }

        /// <summary>
        /// Parse JSON string to Dictionary<string, string>.
        /// </summary>
        /// <param name="mapString">JSON string</param>
        /// <returns>Dictionary or empty dictionary if parsing fails</returns>
        public static Dictionary<string, string> ToStringMap(string? mapString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mapString))
                    return new Dictionary<string, string>();

                return JsonSerializer.Deserialize<Dictionary<string, string>>(mapString, GetJsonOptions()) 
                       ?? new Dictionary<string, string>();
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Parse JSON string to Dictionary<string, object>.
        /// </summary>
        /// <param name="mapString">JSON string</param>
        /// <returns>Dictionary or empty dictionary if parsing fails</returns>
        public static Dictionary<string, object> ToObjectMap(string? mapString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mapString))
                    return new Dictionary<string, object>();

                return JsonSerializer.Deserialize<Dictionary<string, object>>(mapString, GetJsonOptions()) 
                       ?? new Dictionary<string, object>();
            }
            catch
            {
                return new Dictionary<string, object>();
            }
        }

        /// <summary>
        /// Deserialize JSON string to typed object.
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="json">JSON string</param>
        /// <returns>Deserialized object</returns>
        /// <exception cref="JsonException">Thrown when deserialization fails</exception>
        public static T Entity<T>(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json, GetJsonOptions())!;
            }
            catch (Exception e)
            {
                throw new JsonException("Failed to deserialize JSON", e);
            }
        }

        /// <summary>
        /// Serialize object to JSON string.
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>JSON string</returns>
        public static string ToJson(object obj)
        {
            try
            {
                return JsonSerializer.Serialize(obj, GetJsonOptions());
            }
            catch (Exception e)
            {
                throw new JsonException("Failed to serialize object", e);
            }
        }

        /// <summary>
        /// Deserialize JSON array to List<T>.
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="json">JSON array string</param>
        /// <returns>List of objects</returns>
        public static List<T> EntityListFromJson<T>(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<List<T>>(json, GetJsonOptions()) 
                       ?? new List<T>();
            }
            catch (Exception e)
            {
                throw new JsonException("Failed to deserialize JSON array", e);
            }
        }

        /// <summary>
        /// Flatten nested JSON into a flat dictionary with delimiter-separated keys.
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <param name="prefix">Prefix for keys</param>
        /// <param name="delimiter">Delimiter for nested keys</param>
        /// <returns>Flattened dictionary</returns>
        public static Dictionary<string, object> Flatten(string json, string prefix, string delimiter)
        {
            var result = new Dictionary<string, object>();
            try
            {
                using var doc = JsonDocument.Parse(json);
                FlattenHelper(doc.RootElement, prefix, delimiter, result);
                return result;
            }
            catch (JsonException)
            {
                throw new ArgumentException($"Invalid JSON: {json}");
            }
        }

        private static void FlattenHelper(JsonElement element, string prefix, string delimiter, 
            Dictionary<string, object> result)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var property in element.EnumerateObject())
                    {
                        var newPrefix = string.IsNullOrEmpty(prefix) 
                            ? property.Name 
                            : $"{prefix}{delimiter}{property.Name}";
                        FlattenHelper(property.Value, newPrefix, delimiter, result);
                    }
                    break;

                case JsonValueKind.Array:
                    var list = new List<object>();
                    foreach (var item in element.EnumerateArray())
                    {
                        list.Add(ConvertValue(item));
                    }
                    result[prefix] = list;
                    break;

                default:
                    result[prefix] = ConvertValue(element);
                    break;
            }
        }

        private static object ConvertValue(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString()!,
                JsonValueKind.Number => element.TryGetInt32(out var i) ? i : element.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null!,
                _ => element.GetRawText()
            };
        }
    }
}

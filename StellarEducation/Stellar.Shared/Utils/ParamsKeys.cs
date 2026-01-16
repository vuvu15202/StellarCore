namespace Stellar.Shared.Utils
{
    public static class ParamsKeys
    {
        public const string SEARCH = "search";
        public const string PAGE = "page";
        public const string SIZE = "page_size";
        public const string SORT = "sort";
        public const string FILTER = "filter";

        public const string PREFIX_FROM = "from";
        public const string PREFIX_TO = "to";

        /// <summary>
        /// Build a field name with prefix.
        /// </summary>
        /// <param name="prefix">Prefix (e.g., "from", "to")</param>
        /// <param name="field">Field name</param>
        /// <returns>Formatted field name (e.g., "from.FieldName")</returns>
        public static string GetFieldName(string prefix, string field)
        {
            return $"{prefix}.{StringUtils.Capitalize(field)}";
        }
    }
}

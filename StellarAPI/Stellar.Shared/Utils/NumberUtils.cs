namespace Stellar.Shared.Utils
{
    public static class NumberUtils
    {
        /// <summary>
        /// Parse a string to integer with a default fallback value.
        /// </summary>
        /// <param name="number">The string to parse</param>
        /// <param name="defaultNumber">The default value to return if parsing fails</param>
        /// <returns>Parsed integer or default value</returns>
        public static int ParseInt(string? number, int defaultNumber)
        {
            if (int.TryParse(number, out var result))
            {
                return result;
            }

            return defaultNumber;
        }
    }
}

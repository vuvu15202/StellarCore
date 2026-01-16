using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Stellar.Shared.Utils
{
    public static class KeywordReplacer
    {
        private static readonly Regex DefaultPattern = new Regex(@"#(\w+)#", RegexOptions.Compiled);

        /// <summary>
        /// Replace keywords in a string using a custom pattern.
        /// </summary>
        /// <param name="input">Input string with keywords</param>
        /// <param name="patternString">Regex pattern to match keywords</param>
        /// <param name="replacements">Dictionary of replacements</param>
        /// <returns>String with keywords replaced</returns>
        public static string ReplaceKeywords(string input, string patternString, Dictionary<string, string> replacements)
        {
            if (string.IsNullOrEmpty(input) || replacements == null)
                return input;

            var pattern = new Regex(patternString);
            
            return pattern.Replace(input, match =>
            {
                var key = match.Groups[1].Value;
                return replacements.TryGetValue(key, out var replacement) 
                    ? replacement 
                    : match.Value;
            });
        }

        /// <summary>
        /// Replace keywords in a string using the default pattern (#keyword#).
        /// </summary>
        /// <param name="input">Input string with keywords</param>
        /// <param name="replacements">Dictionary of replacements</param>
        /// <returns>String with keywords replaced</returns>
        public static string ReplaceKeywords(string input, Dictionary<string, string> replacements)
        {
            return ReplaceKeywords(input, @"#(\w+)#", replacements);
        }
    }
}

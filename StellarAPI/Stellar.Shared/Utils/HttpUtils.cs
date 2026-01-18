using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Stellar.Shared.Utils
{
    public static class HttpUtils
    {
        /// <summary>
        /// Extract all headers from an HttpRequest into a dictionary.
        /// </summary>
        /// <param name="request">The HTTP request</param>
        /// <returns>Dictionary of header names and values</returns>
        public static Dictionary<string, string> GetHeaderMap(HttpRequest request)
        {
            var headers = new Dictionary<string, string>();
            
            foreach (var header in request.Headers)
            {
                headers[header.Key] = header.Value.ToString();
            }

            return headers;
        }
    }
}

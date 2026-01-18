using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Extensions.Http;

namespace Stellar.Shared.Utils
{
    public static class RequestUtils
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const int DEFAULT_RETRY_ATTEMPTS = 3;
        private static readonly TimeSpan RETRY_DELAY = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Make an HTTP request with retry logic for server errors.
        /// </summary>
        /// <param name="url">Target URL</param>
        /// <param name="method">HTTP method</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="body">Request body object</param>
        /// <param name="headers">Request headers</param>
        /// <returns>JSON response with statusCode field</returns>
        public static async Task<string> Request(
            string url,
            HttpMethod method,
            Dictionary<string, object>? parameters = null,
            object? body = null,
            Dictionary<string, string>? headers = null)
        {
            var finalUrl = url;

            // Append query parameters if provided
            if (parameters != null && parameters.Any())
            {
                var queryString = string.Join("&", 
                    parameters.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value?.ToString() ?? "")}"));
                
                finalUrl += url.Contains("?") ? $"&{queryString}" : $"?{queryString}";
            }

            var request = new HttpRequestMessage(method, finalUrl);

            // Add headers
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            request.Headers.TryAddWithoutValidation("Content-Type", "application/json");

            // Add body if provided
            if (body != null)
            {
                var jsonBody = JsonParserUtils.ToJson(body);
                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            }

            // Retry policy for 5xx errors
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => (int)msg.StatusCode >= 500)
                .WaitAndRetryAsync(DEFAULT_RETRY_ATTEMPTS, _ => RETRY_DELAY);

            var response = await retryPolicy.ExecuteAsync(async () => 
                await _httpClient.SendAsync(request));

            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (string.IsNullOrWhiteSpace(responseBody))
            {
                responseBody = "{}";
            }

            // Add status code to response
            var bodyMap = JsonParserUtils.ToObjectMap(responseBody);
            bodyMap["statusCode"] = (int)response.StatusCode;

            return JsonParserUtils.ToJson(bodyMap);
        }
    }
}

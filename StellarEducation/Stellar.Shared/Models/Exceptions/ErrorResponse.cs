using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Stellar.Shared.Models.Exceptions
{
    public class ErrorResponse
    {
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("messageCode")]
        public string MessageCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("traceId")]
        public string TraceId { get; set; }

        [JsonPropertyName("errors")]
        public List<FieldErrorDetail> Errors { get; set; }

        public class FieldErrorDetail
        {
            [JsonPropertyName("field")]
            public string Field { get; set; }

            [JsonPropertyName("message")]
            public string Message { get; set; }

            [JsonPropertyName("rejectedValue")]
            public object RejectedValue { get; set; }

            public FieldErrorDetail() { }
            public FieldErrorDetail(string field, string message, object rejectedValue)
            {
                Field = field;
                Message = message;
                RejectedValue = rejectedValue;
            }
        }
    }
}

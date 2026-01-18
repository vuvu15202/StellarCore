using System;

namespace Stellar.Shared.Models.Exceptions
{
    public class ResponseException : Exception
    {
        public int StatusCode { get; set; }
        public string MessageCode { get; set; }

        public ResponseException(string message) : base(message)
        {
            StatusCode = 500;
            MessageCode = CommonErrorMessage.INTERNAL_SERVER;
        }

        public ResponseException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
            MessageCode = CommonErrorMessage.INTERNAL_SERVER;
        }

        public ResponseException(string message, string messageCode, int statusCode = 400) : base(message)
        {
            StatusCode = statusCode;
            MessageCode = messageCode;
        }
    }
}

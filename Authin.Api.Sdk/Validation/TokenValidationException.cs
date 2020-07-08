using System;

namespace Authin.Api.Sdk.Validation
{
    public class TokenValidationException : Exception
    {
        public TokenValidationException(string message)
            : base(message)
        {
        }

        public TokenValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
using System;
using System.Net;

namespace FireSharp.Core.Exceptions
{
    public class FirebaseException : Exception
    {
        public FirebaseException(string message)
            : base(message)
        {
        }

        public FirebaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public FirebaseException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        public FirebaseException(HttpStatusCode statusCode, string responseBody)
            : base($"Request responded with status code={statusCode}, response={responseBody}")
        {
        }
    }
}
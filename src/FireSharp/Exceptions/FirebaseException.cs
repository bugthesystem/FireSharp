using System;

namespace FireSharp.Exceptions
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
    }
}
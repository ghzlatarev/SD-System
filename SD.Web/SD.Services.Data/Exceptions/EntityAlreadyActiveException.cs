using System;

namespace SD.Services.Data.Exceptions
{
    public class EntityAlreadyActiveException : Exception
    {
        public EntityAlreadyActiveException()
        {
        }

        public EntityAlreadyActiveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

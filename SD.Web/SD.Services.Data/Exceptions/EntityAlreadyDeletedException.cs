using System;

namespace SD.Services.Data.Exceptions
{
    public class EntityAlreadyDeletedException : Exception
    {
        public EntityAlreadyDeletedException()
        {
        }

        public EntityAlreadyDeletedException(string message) : base(message)
        {
        }
    }
}

using System;

namespace ActivityLogger.Infrastructure.Exceptions
{
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException(string message) : base(message)
        {
        }
    }
}
using System;

namespace LaserPointer.WebApi.Application.Common.Exceptions
{
    public class UserLimitException : Exception
    {
        public UserLimitException() : base()
        {
        }
        
        public UserLimitException(string name, string userId)
            : base($"Entity \"{name}\" limit reached for user {userId}.")
        {
        }
        
        public UserLimitException(string message)
            : base(message)
        {
        }
    }
}

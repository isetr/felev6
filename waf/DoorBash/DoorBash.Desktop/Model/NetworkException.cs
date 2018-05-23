using System;

namespace DoorBash.Desktop.Model
{
    public class NetworkException : Exception
    {
        public NetworkException(String message) : base(message)
        {
        }

        public NetworkException(Exception innerException) : base("Exception occurred.", innerException)
        {
        }
    }
}
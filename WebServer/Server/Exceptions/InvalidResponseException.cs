namespace WebServer.Server.Exceptions
{
    using System;

    public class InvalidResponseException : Exception
    {
        private const string ErrorMessage = "View responses need a status code below 300 and above 400(inclusive)";

        public InvalidResponseException()
            : base(ErrorMessage)
        {
        }

        public InvalidResponseException(string message)
            : base(message)
        {
        }
    }
}
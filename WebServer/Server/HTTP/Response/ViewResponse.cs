namespace WebServer.Server.HTTP.Response
{
    using Enums;
    using Exceptions;
    using Contracts;
    
    public class ViewResponse : HttpResponse
    {
        private readonly IView view;

        public ViewResponse(ResponseStatusCode statusCode, IView view)
        {
            ValidateStatusCode(statusCode);
            this.view = view;
            this.StatusCode = statusCode;
        }

        private void ValidateStatusCode(ResponseStatusCode statusCode)
        {
            var statusNumber = (int) statusCode;
            if (299 < statusNumber && statusNumber < 400)
            {
                throw new InvalidResponseException();
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}{this.view.View()}";
        }
    }
}
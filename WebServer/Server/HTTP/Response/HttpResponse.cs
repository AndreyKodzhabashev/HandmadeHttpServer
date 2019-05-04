namespace WebServer.Server.HTTP.Response
{
    using Common;
    using Enums;
    using Contracts;
    using System.Text;

    public abstract class HttpResponse : IHttpResponse
    {
        protected HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
        }

        public HttpHeaderCollection Headers { get; set; }

        public ResponseStatusCode StatusCode { get; protected set; }

        public string StatusCodeMessage => this.StatusCodeMessage.ToString();

        public override string ToString()
        {
            var response = new StringBuilder();
            var statusCodeNumber = (int) this.StatusCode;
            response.AppendLine($"HTTP/1.1 {statusCodeNumber} {StatusCodeMessage}");
            //TODO problem
            response.AppendLine(this.Headers.ToString());

            return response.ToString().TrimEnd();
        }
    }
}
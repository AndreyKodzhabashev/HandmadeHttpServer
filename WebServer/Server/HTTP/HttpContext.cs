namespace WebServer.Server.HTTP
{
    using Contracts;

    public class HttpContext : IHttpContext
    {
        public HttpContext(string requestStr)
        {
            this.Request = new HttpRequest(requestStr);
        }

        public HttpRequest Request { get; }
    }
}
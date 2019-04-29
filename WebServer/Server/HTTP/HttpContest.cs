namespace WebServer.Server.HTTP
{
    using Contracts;

    public class HttpContest : IHttpContext
    {
        public HttpContest(string requestStr)
        {
            this.Request = new HttpRequest(requestStr);
        }

        public HttpRequest Request { get; }
    }
}
namespace WebServer.Server.HTTP.Response
{
    using Contracts;

    public abstract class HttpResponse : IHttpResponse
    {
        protected HttpResponse(string redirectUrl)
        {

        }
    }
}
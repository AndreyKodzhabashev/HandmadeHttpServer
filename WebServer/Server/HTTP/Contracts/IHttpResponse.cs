namespace WebServer.Server.HTTP.Contracts
{
    using Enums;

    public interface IHttpResponse
    {
        ResponseStatusCode StatusCode { get; }
        HttpHeaderCollection Headers { get; }
    }
}
using WebServer.Server.Common;

namespace WebServer.Server.HTTP
{
    using Contracts;

    public class HttpContext : IHttpContext
    {
        private readonly IHttpRequest request;

        public HttpContext(IHttpRequest requestStr)
        {
            CoreValidator.ThrowIfNull(requestStr, nameof(requestStr));

            this.request = requestStr;
        }

        public IHttpRequest Request => this.request;
    }
}
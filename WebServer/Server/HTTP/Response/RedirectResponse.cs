namespace WebServer.Server.HTTP.Response
{
    using Common;
    using Enums;

    class RedirectResponse : HttpResponse
    {
        public RedirectResponse(string redirectUrl)
        {
            CoreValidator.ThrowIfNullOrEmpty(redirectUrl, nameof(redirectUrl));

            this.StatusCode = ResponseStatusCode.Found;
            this.Headers.Add(new HttpHeader("Location", redirectUrl));
        }
    }
}
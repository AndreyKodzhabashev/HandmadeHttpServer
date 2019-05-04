namespace WebServer.Server.HTTP
{
    using Common;
    using Contracts;
    using Enums;
    using Exceptions;
    using System;
    using System.Linq;
    using System.Net;
    using System.Collections.Generic;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestText)
        {
            CoreValidator.ThrowIfNull(requestText, nameof(requestText));

            this.FormData = new Dictionary<string, string>();
            this.HeaderCollection = new HttpHeaderCollection();
            this.QueryParameters = new Dictionary<string, string>();
            this.UrlParameters = new Dictionary<string, string>();

            this.ParseRequest(requestText);
        }

        public Dictionary<string, string> FormData { get; }

        public HttpHeaderCollection HeaderCollection { get; }

        public string Path { get; private set; }

        public Dictionary<string, string> QueryParameters { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, string> UrlParameters { get; }

        public void AddUrlParameter(string key, string value)
        {
            this.UrlParameters[key] = value;
        }

        private void ParseRequest(string requestText)
        {
            string[] requestLines = requestText.Split(Environment.NewLine, StringSplitOptions.None);

            string[] requestLine = requestLines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (requestLine.Length != 3 || requestLine[2].ToLower() != "http/1.1")
            {
                throw new BadRequestException("Invalid Request line");
            }

            this.RequestMethod = ParseRequestMethod(requestLine[0].ToUpper());

            this.Url = requestLine[1];
            this.Path = this.Url.Split(new[] {'?', '#'}, StringSplitOptions.RemoveEmptyEntries)[0];

            this.ParseHeaders(requestLines);
            this.ParseParameters();

            if (this.RequestMethod == HttpRequestMethod.POST)
            {
                this.ParseQuery(requestLines.Last(), this.FormData);
            }
        }

        private HttpRequestMethod ParseRequestMethod(string requestMethod)
        {
            if (Enum.TryParse(requestMethod, out HttpRequestMethod result))
            {
                return result;
            }

            throw new BadRequestException("Request method is not GET or POST");
        }

        private void ParseHeaders(string[] requestLines)
        {
            int endIndex = Array.IndexOf(requestLines, String.Empty);

            for (int i = 1; i < endIndex; i++)
            {
                string[] headerArgs = requestLines[i].Split(": ", StringSplitOptions.None);

                if (headerArgs.Length != 2)
                {
                    throw new BadRequestException($"Invalid header {i} in request");
                }

                var currentHeader = new HttpHeader(headerArgs[0], headerArgs[1]);
                HeaderCollection.Add(currentHeader);
            }

            if (HeaderCollection.ContainsKey("Host") == false)
            {
                throw new BadRequestException("Host header must persist.");
            }
        }

        private void ParseParameters()
        {
            if (this.Url.Contains("?") == false)
            {
                return;
            }

            string query = this.Url.Split("?")[1];

            this.ParseQuery(query, this.UrlParameters);
        }

        private void ParseQuery(string query, IDictionary<string, string> dictionary)
        {
            if (query.Contains("=") == false)
            {
                return;
            }

            string[] queryPairs = query.Split("&", StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in queryPairs)
            {
                string[] queryArgs = pair.Split("=", StringSplitOptions.RemoveEmptyEntries);
                if (queryArgs.Length != 2)
                {
                    continue;
                }

                dictionary.Add(WebUtility.UrlDecode(queryArgs[0]),
                    WebUtility.UrlDecode(queryArgs[1]));
            }
        }
    }
}
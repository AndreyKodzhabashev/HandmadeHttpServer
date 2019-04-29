using System;
using System.Net;
using WebServer.Server.Exceptions;

namespace WebServer.Server.HTTP
{
    using Contracts;
    using Enums;
    using System.Collections.Generic;

    public class HttpRequest : IHttpRequest
    {
        private Dictionary<string, string> _formData;
        private HttpHeaderCollection _headerCollection;
        private string _path;
        private Dictionary<string, string> _queryParameters;
        private HttpRequestMethod _requestMethod;
        private string _url;
        private Dictionary<string, string> _urlParameters;

        public HttpRequest(string requestString)
        {
            this._formData = new Dictionary<string, string>();
            this._headerCollection = new HttpHeaderCollection();
            this._queryParameters = new Dictionary<string, string>();
            this._urlParameters = new Dictionary<string, string>();

            this.ParseRequest(requestString);
        }

        public Dictionary<string, string> FormData => _formData;

        public HttpHeaderCollection HeaderCollection => _headerCollection;

        public string Path => _path;

        public Dictionary<string, string> QueryParameters => _queryParameters;

        public HttpRequestMethod RequestMethod => _requestMethod;

        public string Url => _url;

        public Dictionary<string, string> UrlParameters => _urlParameters;

        public void AddUrlParameter(string key, string value)
        {
            throw new System.NotImplementedException();
        }

        private void ParseRequest(string requestString)
        {
            string[] requestLines = requestString.Split(Environment.NewLine, StringSplitOptions.None);

            string[] requestLine = requestLines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (requestLine.Length != 3 || requestLine[2].ToLower() != "http/1.1")
            {
                throw new BadRequestException("Invalid Request line");
            }

            this._requestMethod = ParseRequestMethod(requestLine[0].ToUpper());

            this._url = requestLine[1];
            this._path = this.Url.Split(new[] {'?', '#'}, StringSplitOptions.RemoveEmptyEntries)[0];

            this.ParseHeaders(requestLines);
            this.ParseParameters();

            if (this.RequestMethod == HttpRequestMethod.POST)
            {
                this.ParseQuery(requestLines[requestLines.Length - 1], this._formData);
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

                var currentHeader = new HttpHeader(headerArgs[0], headerArgs[1]);
                _headerCollection.Add(currentHeader);
            }

            if (HeaderCollection.ContainsKey("Host") == false)
            {
                ?throw new BadRequestException("Host header must persist.");
            }
        }

        private void ParseParameters()
        {
            if (this.Url.Contains("?") == false)
            {
                return;
            }

            string query = Url.Split("?")[1];

            this.ParseQuery(query, this._queryParameters);
        }

        private void ParseQuery(string query, Dictionary<string, string> queryParameters)
        {
            if (query.Contains("=") == false)
            {
                return;
            }

            string[] queryPairs = query.Split("&");

            foreach (var pair in queryPairs)
            {
                string[] queryArgs = pair.Split("=");
                if (queryArgs.Length != 2)
                {
                    continue;
                }

                queryParameters.Add(
                    WebUtility.UrlDecode(queryArgs[0]),
                    WebUtility.UrlDecode(queryArgs[1]));
            }
        }
    }
}
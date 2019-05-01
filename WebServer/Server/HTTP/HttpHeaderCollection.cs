namespace WebServer.Server.HTTP
{
    using Contracts;
    using System;
    using System.Collections.Generic;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            headers[header.Key] = header;
        }

        public bool ContainsKey(string key)
        {
            return headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            if (headers.ContainsKey(key))
            {
                return headers[key];
            }

            return null;
        }

        public override string ToString() => string.Join(Environment.NewLine, this.headers);
        
    }
}
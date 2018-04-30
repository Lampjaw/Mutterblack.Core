using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Mutterblack.Core.HttpAuthenticatedClient
{
    public class AuthenticatedHttpClientOptions
    {
        public AuthenticatedHttpClientOptions()
        {
            Scopes = new List<string>();
        }

        public string TokenServiceAddress { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public List<string> Scopes { get; set; }

        public HttpMessageHandler InnerMessageHandler { get; set; }
        public HttpMessageHandler TokenServiceMessageHandler { get; set; }

        public int TokenServiceTimeout { get; set; } = 10000;
        public int MessageHandlerTimeout { get; set; } = 60000;

        public string GetTokenServiceServer()
        {
            var uri = new Uri(TokenServiceAddress);
            return $"{uri.Scheme}://{uri.Authority}";
        }
    }
}

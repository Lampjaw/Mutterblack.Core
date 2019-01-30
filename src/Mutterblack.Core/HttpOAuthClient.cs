using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mutterblack.Core
{
    public class HttpOAuthClient : HttpClient
    {
        private readonly DateTime _epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private readonly HMACSHA1 _sigHasher;
        private readonly string _oAuthKey;

        public HttpOAuthClient(string oAuthKey, string oAuthSecret)
        {
            _oAuthKey = oAuthKey;
            _sigHasher = new HMACSHA1(new ASCIIEncoding().GetBytes(string.Format("{0}&", oAuthSecret)));
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken, params KeyValuePair<string, string>[] oAuthParams)
        {
            return SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri), cancellationToken, oAuthParams);
        }

        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendAsync(request, cancellationToken);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken, params KeyValuePair<string, string>[] oAuthParams)
        {
            var fullUri = new Uri(BaseAddress, request.RequestUri);
            var absoluteUri = $"{fullUri.Scheme}://{fullUri.Host}{fullUri.AbsolutePath}";

            request.Headers.Authorization = new AuthenticationHeaderValue("OAuth", GetAuthorizationHeader(absoluteUri, oAuthParams));

            return base.SendAsync(request, cancellationToken);
        }

        private string GetAuthorizationHeader(string url, params KeyValuePair<string, string>[] oAuthParams)
        {
            var nonce = Guid.NewGuid().ToString("N");
            var timestamp = ((int)(DateTime.UtcNow - _epochUtc).TotalSeconds).ToString();

            var data = new Dictionary<string, string>();

            data.Add("oauth_consumer_key", _oAuthKey);
            data.Add("oauth_signature_method", "HMAC-SHA1");
            data.Add("oauth_timestamp", timestamp);
            data.Add("oauth_nonce", nonce);
            data.Add("oauth_version", "1.0");
            data.Add("format", "json");

            oAuthParams.ToList().ForEach(param => data[param.Key] = param.Value);

            data.Add("oauth_signature", GenerateSignature(url, data));

            return GenerateOAuthHeader(data);
        }

        private string GenerateOAuthHeader(Dictionary<string, string> data)
        {
            return string.Join(
                ",",
                data
                    .Where(kvp => kvp.Key.StartsWith("oauth_"))
                    .Select(kvp => string.Format("{0}=\"{1}\"", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                    .OrderBy(s => s)
            );
        }

        private string GenerateSignature(string url, Dictionary<string, string> data)
        {
            var sigString = string.Join(
                "&",
                data
                    .Union(data)
                    .Select(kvp => string.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                    .OrderBy(s => s)
            );

            var fullSigData = string.Format(
                "{0}&{1}&{2}",
                "GET",
                Uri.EscapeDataString(url),
                Uri.EscapeDataString(sigString.ToString())
            );

            return Convert.ToBase64String(_sigHasher.ComputeHash(new ASCIIEncoding().GetBytes(fullSigData.ToString())));
        }
    }
}

using System.Net.Http;

namespace Mutterblack.Core.HttpAuthenticatedClient
{
    public interface IAuthenticatedHttpClientFactory
    {
        HttpClient GetHttpClient();
    }
}

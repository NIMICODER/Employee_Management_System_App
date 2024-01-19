

using Ems_Shared.Dtos;

namespace Ems_UI_Service.Helpers
{
    public class GetHttpClient(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
    {  
        private const string HeaderKey = "Authorization";

        public async Task<HttpClient> GetPrivateHttpClient()
        {
            var client = httpClientFactory.CreateClient("SystemApiClient");
            var stringToken = await localStorageService.GetToken();
            if (string.IsNullOrEmpty(stringToken)) return client;

            var deserializeToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
            if(deserializeToken == null) return client;

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializeToken.Token);
            return client; 
        }

        public HttpClient GetPublicHttpCliet()
        {
            var client = httpClientFactory.CreateClient("SystemApiClient");
            client.DefaultRequestHeaders.Remove(HeaderKey);
            return client;  
        }
    }
}

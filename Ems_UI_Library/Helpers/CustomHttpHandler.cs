

using Ems_Shared.Dtos;
using Ems_UI_Service.Interfaces;

namespace Ems_UI_Service.Helpers
{
    public class CustomHttpHandler(GetHttpClient getHttpClient, LocalStorageService localStorageService, IAuthenticationService authenticationService) : DelegatingHandler
    {
       protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool loginUrl = request.RequestUri!.AbsoluteUri.Contains("loginUser");
            bool registerUrl = request.RequestUri!.AbsoluteUri.Contains("registerUser");
            bool refreshTokenUrl = request.RequestUri!.AbsoluteUri.Contains("refreshToken");

            if(loginUrl && registerUrl && refreshTokenUrl) return await base.SendAsync(request, cancellationToken);  
            var result = await base.SendAsync(request, cancellationToken);
            if(result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                //Get token from localStorage
                var stringToken = await localStorageService.GetToken();
                if (stringToken == null) return result;
                //Check if header contains token
                string token = string.Empty;
                try { token = request.Headers.Authorization!.Parameter!; } 
                catch { }

                //to decrypt the token above
                var deserializedToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
                if(deserializedToken == null) return result;    
                if (string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializedToken.Token);
                    return await base.SendAsync(request, cancellationToken);    
                }

                //Call for refresh token
                var newJwtToken = await GetRefreshToken(deserializedToken.RefreshToken!);
                if(string.IsNullOrEmpty(newJwtToken)) return result;

                //if 401, it will pause/ call the refreshToken API/ get the token/add to local storage/ remove the old one/ append a new hearder to the httpClient/ remake same call to current data we want
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializedToken.Token);
                return await base.SendAsync(request, cancellationToken);
            }
            return result;
       } 

        private async Task<string> GetRefreshToken(string refreshToken) {
            var result = await authenticationService.RefreshTokenAsync(new RefreshToken()
            {
                Token = refreshToken
            });
            string serializeToken = Serializations.SerializeObj(new UserSession()
            { Token = result.Token, RefreshToken = result.RefreshToken });
            await localStorageService.SetToken(serializeToken);
            return result.Token;
        }
    }
}

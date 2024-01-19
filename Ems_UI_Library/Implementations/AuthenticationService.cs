using Ems_Shared.Dtos;
using Ems_Shared.Dtos.Responses;
using Ems_UI_Service.Helpers;
using Ems_UI_Service.Interfaces;
using System.Net.Http.Json;

namespace Ems_UI_Service.Implementations
{
    public class AuthenticationService(GetHttpClient getHttpClient) : IAuthenticationService
    {
        public const string authUrl = "api/Authentication";
        public async Task<GeneralResponse> CreateAccountAsync(Register userRequest)
        {
            var httpClient = getHttpClient.GetPublicHttpCliet();
            var result = await httpClient.PostAsJsonAsync($"{authUrl}/registerUser", userRequest);
            if(!result.IsSuccessStatusCode)
            {
                return new GeneralResponse(false, "An Error occured");
            }
            return await result.Content.ReadFromJsonAsync<GeneralResponse>();   
        }

        public async Task<LoginResponse> LoginAccountAsync(Login userRequest)
        {
            var httpClient = getHttpClient.GetPublicHttpCliet();
            var result = await httpClient.PostAsJsonAsync($"{authUrl}/loginUser", userRequest);
            if (!result.IsSuccessStatusCode)
            {
                return new LoginResponse(false, "An Error occured");
            }
            return await result.Content.ReadFromJsonAsync<LoginResponse>();
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
        {
            var httpClient = getHttpClient.GetPublicHttpCliet();
            var result = await httpClient.PostAsJsonAsync($"{authUrl}/refreshToken", token);
            if (!result.IsSuccessStatusCode)
            {
                return new LoginResponse(false, "An Error occured");
            }
            return await result.Content.ReadFromJsonAsync<LoginResponse>();
        }



        public async Task<WeatherForecast[]> GetWeatherForcasts()
        {
            var httpClient = await getHttpClient.GetPrivateHttpClient();
            var result = await httpClient.GetFromJsonAsync<WeatherForecast[]>("api/WeatherForecast");
            return result!;
        }
    }
}

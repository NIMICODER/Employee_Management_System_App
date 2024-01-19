using Ems_Shared.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ems_UI_Service.Helpers
{
    //getting data from local storage and decrypting it/ visualize the token/ get the claims in it/ set it to claim principal and return it
    public class CustomAuthenticationStateProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        //this method checks user authentication when user switch between pages
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var stringToken = await localStorageService.GetToken();
            if (string.IsNullOrEmpty(stringToken)) return await Task.FromResult(new AuthenticationState(anonymous));

            var deserializeToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
            if (deserializeToken == null) return await Task.FromResult(new AuthenticationState(anonymous));

            var getUserClaims = DecryptToken(deserializeToken.Token!);
            if (getUserClaims == null) return await Task.FromResult(new AuthenticationState(anonymous));

            //after getting claim/ this method pass the claim to authentication/ and  tells if user is authenticated because of the claims you now have
            var claimsPrincipal = SetClaimPrincipal(getUserClaims);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        //when a user try to login we want to set the token to a local storage/ call the setclaim to set the claim to token/ and refresh the page
        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if(userSession.Token != null || userSession.RefreshToken != null)
            {
                var serializeSession = Serializations.SerializeObj(userSession);
                await localStorageService.SetToken(serializeSession);
                var getUserClaims = DecryptToken(userSession.Token!);    
                claimsPrincipal = SetClaimPrincipal(getUserClaims); 
            }
            else
            {
                await localStorageService.RemoveToken();
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims claims)
        {
            if (claims.Email is null) return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity(
            new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, claims.Id!),
                new Claim(ClaimTypes.Name, claims.Name!),
                new Claim(ClaimTypes.Email, claims.Email!),
                new Claim(ClaimTypes.Role, claims.Role)
            }, "JwtAuth"));

        }

        private static CustomUserClaims DecryptToken(string jwtToken)
        {
            if(string.IsNullOrEmpty(jwtToken)) return new CustomUserClaims();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var userId = token.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);
            var name = token.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name);
            var email = token.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Email);
            var role = token.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Role);
            return new CustomUserClaims(userId!.Value, name!.Value, email!.Value, role!.Value);
        }

        

    }
}

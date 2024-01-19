using Ems_Data.Context;
using Ems_Models.Entities;
using Ems_Models.Identity;
using Ems_Services.Helper;
using Ems_Services.Interfaces;
using Ems_Shared.Dtos;
using Ems_Shared.Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Constants = Ems_Services.Helper.Constants;

namespace Ems_Services.Implementations
{
    public class AuthenticationService(IOptions<JwtConfig> config, ApplicationDbContext context ) : IAuthenticationService
    {
        public async Task<GeneralResponse> CreateAccountAsync(Register userRequest)
        {
            if (userRequest == null)
            {
                return new GeneralResponse(false, "user entry is empty");
            }
            var checkUser =  await FindUserByEmail(userRequest.Email);
            if (checkUser != null) return new GeneralResponse(false, "User already registered");
            // save User
            var appUser = await AddToDatabase(new ApplicationUser()
            {
                FullName = userRequest.FullName,
                Email = userRequest.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password)
            });
            //check, create and assign role
            var checkAdmiRole = await context.SystemRoles.FirstOrDefaultAsync(r => r.Name!.Equals(Constants.Admin));
            if (checkAdmiRole == null)
            {
                var createAdminRole = await AddToDatabase(new EmsRoles() { Name = Constants.Admin });
                await AddToDatabase(new UserRoles() { RoleId = createAdminRole.Id, UserId = appUser.Id });
                return new GeneralResponse(false, " User Account created");

            }
            var checkUserRole = await context.SystemRoles.FirstOrDefaultAsync(ur => ur.Name!.Equals(Constants.User));  
            EmsRoles response = new EmsRoles(); 
            if (checkUserRole == null)
            {
                response = await AddToDatabase(new EmsRoles() { Name= Constants.User });
                await AddToDatabase(new UserRoles() { RoleId= response.Id, UserId= appUser.Id });
            }
            else
            {
                await AddToDatabase(new UserRoles() { RoleId = checkUserRole.Id, UserId = appUser.Id });
            }
            return new GeneralResponse(true, "User Acoount created");
        }


        public async Task<LoginResponse> LoginAccountAsync(Login userRequest)
        {
            if(userRequest is null)
            {
                return new LoginResponse(false, "model is empty");
            }
            var appUser = await FindUserByEmail(userRequest.Email!);
            if (appUser == null)
            {
                return new LoginResponse(false, "User not found");
            }

            //Verify password
            if (!BCrypt.Net.BCrypt.Verify(userRequest.Password, appUser.Password))
                return new LoginResponse(false, " Invalid email or Password");

            var getUserRole = await FindUserRole(appUser.Id);
            if (getUserRole == null)
            {
                return new LoginResponse(false, "User role not found");
            }
            var getRoleName = await FindRoleName(getUserRole.RoleId);
            if (getUserRole == null)
            {
                return new LoginResponse(false, "User role not found");
            }
            string jwtToken = GenerateAccessToken(appUser, getRoleName!.Name!);
            string refreshToken = GenerateRefreshToken();

            //Save the Refresh token to db
            var findUser = await context.RefreshTokenInfos.FirstOrDefaultAsync(u => u.UserId == appUser.Id);
            if (findUser is not null)
            {
                findUser!.Token = refreshToken;
                await context.SaveChangesAsync();
            } else
            {
                await AddToDatabase(new RefreshTokenInfo()
                {
                    Token = refreshToken,
                    UserId = appUser.Id
                });
            }
            return new LoginResponse(true, "Login Successfully", jwtToken, refreshToken);
        }

        private string GenerateAccessToken(ApplicationUser user, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.JwtKey!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role!)
            };
            var token = new JwtSecurityToken(
                issuer: config.Value.JwtIssuer,
                audience: config.Value.JwtAudience,
                claims: userClaims,
                expires: DateTime.Now.AddSeconds(15),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private async Task<UserRoles> FindUserRole(int userId) => 
            await context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId);
        private async Task<EmsRoles> FindRoleName(int roleId) => 
            await context.SystemRoles.FirstOrDefaultAsync(sr => sr.Id == roleId);
        private async Task<ApplicationUser> FindUserByEmail(string email) => 
            await context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email!.ToLower()!.Equals(email!.ToLower()));

        private async Task<T> AddToDatabase<T>(T model)
        {
            var result = context.Add(model!);
            await context.SaveChangesAsync();
            return (T)result.Entity;
        }

        public async Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
        {
            if (token is null) return new LoginResponse(false, "token is empty");

            var findToken = await context.RefreshTokenInfos.FirstOrDefaultAsync(t => t.Token!.Equals(token.Token));
            if (findToken is null) return new LoginResponse(false, "Refresh token is required");

            //get user details
            var user = await context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == findToken.UserId);
            if (user is null) return new LoginResponse(false,"Refresh Token could not be generated because user not found");

            var userRole = await FindUserRole(user.Id);
            var roleName = await FindRoleName(userRole.RoleId);
            string jwtToken = GenerateAccessToken(user, roleName.Name!);
            string refreshToken = GenerateRefreshToken();

            var updateRefreshToken = await context.RefreshTokenInfos.FirstOrDefaultAsync(u => u.UserId == user.Id);
            if (updateRefreshToken is null) return new LoginResponse(false, "Refresh Token could not be generated because user not Signed In");

            updateRefreshToken.Token = refreshToken;
            await context.SaveChangesAsync();
            return new LoginResponse(true, "Token refreshed successfully", jwtToken, refreshToken);

        }
    }
}

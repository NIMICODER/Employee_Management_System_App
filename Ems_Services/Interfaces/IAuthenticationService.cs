using Ems_Shared.Dtos;
using Ems_Shared.Dtos.Responses;
namespace Ems_Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<GeneralResponse> CreateAccountAsync(Register userDTO);
        Task<LoginResponse> LoginAccountAsync(Login loginDTO);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
    }
}

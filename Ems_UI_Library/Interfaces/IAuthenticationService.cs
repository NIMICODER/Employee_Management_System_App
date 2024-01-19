using Ems_Shared.Dtos;
using Ems_Shared.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems_UI_Service.Interfaces
{
    public interface IAuthenticationService
    {
        Task<GeneralResponse> CreateAccountAsync(Register userDTO);
        Task<LoginResponse> LoginAccountAsync(Login loginDTO);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
        // for Test
        Task<WeatherForecast[]> GetWeatherForcasts();
    }
}

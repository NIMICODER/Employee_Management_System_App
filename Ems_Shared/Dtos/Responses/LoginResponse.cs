namespace Ems_Shared.Dtos.Responses
{
    public record LoginResponse(bool Status, string Message = null!, string Token = null!, string RefreshToken = null!);
}

using System.ComponentModel.DataAnnotations;

namespace Ems_Shared.Dtos.Requests
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.EmailAddress)]
        public string? Password { get; set; }
    }
}

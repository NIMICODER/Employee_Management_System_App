using Ems_Shared.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems_Shared.Dtos
{
    public class Register : RegisterUserDto
    {

        [Required(ErrorMessage = "Full name is required")]
        [MinLength(5)]
        [MaxLength(100)]
        public string? FullName { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Re-enter password to continue")]
        public string? ConfirmPassword { get; set; }

    }
}

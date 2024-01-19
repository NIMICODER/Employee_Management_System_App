using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems_Shared.Dtos
{
    public class UserSession
    {
        public string? Token { get; set; }  
        public string? RefreshToken { get; set;}
    }
}

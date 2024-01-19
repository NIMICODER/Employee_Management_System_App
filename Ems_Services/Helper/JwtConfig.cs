using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems_Services.Helper
{
    public class JwtConfig
    {
        public string? JwtKey { get; set; }
        public string? JwtIssuer { get; set;}
        public string? JwtAudience { get; set; }
        public string? JwtExpireMinutes { get; set;}
    }
}

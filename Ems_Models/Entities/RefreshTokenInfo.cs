using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems_Models.Entities
{
    public class RefreshTokenInfo
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public int UserId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems_Shared.Dtos
{
    public record CustomUserClaims(string Id = null, string Name = null!, string Email = null!, string Role = null!);
}

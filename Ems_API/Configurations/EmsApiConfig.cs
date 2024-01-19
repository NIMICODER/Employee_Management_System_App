using Ems_Services.Helper;

namespace Ems_API.Configurations
{
    public class EmsApiConfig
    {
        public string ConnectionString { get; set; } = null!;
        public JwtConfig JwtConfig { get; set; } = null!;
    }
}

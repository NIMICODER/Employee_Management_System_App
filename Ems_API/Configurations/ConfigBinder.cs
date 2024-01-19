using Ems_Services.Helper;

namespace Ems_API.Configurations
{
    public static class ConfigBinder
    {
        public static IServiceCollection BindConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            JwtConfig jwt = new JwtConfig();


            configuration.GetSection("Jwt").Bind(jwt);

            services.AddSingleton(jwt);

            return services;
        }
    }
}

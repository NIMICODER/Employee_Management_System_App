using Ems_Data.Context;
using Ems_Data.Implementations;
using Ems_Data.Interfaces;
using Ems_Models.Identity;
using Ems_Services.Helper;
using Ems_Services.Implementations;
using Ems_Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Ems_API.Extentions
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork<ApplicationDbContext>>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();    

        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "EmployeeManagementSystem API",
                    Version = "v1",
                    Description = " EmployeeManagementSystemApp API by NimiCoder",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "David Ukpoju",
                        Email = "ukpojuojdave12@gmail.com",
                        Url = new Uri("https://twitter.com/johndoe"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "DemoFintechApp API LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                s.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "EmployeeManagementSystem API",
                    Version = "v2"
                });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                 {
                     {
                         new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                         },
                         new List<string>()
                     }
                 });




            });
        }

        public static void ConfigureCors(this IServiceCollection services) =>
             services.AddCors(options =>
             {
                 options.AddPolicy("AllowBlazorWasm", builder => builder
                 .WithOrigins("http://localhost:5130", "https://localhost:7272")
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials());
             });


        //public static void ConfigureAuthentication(this IServiceCollection services, JwtConfig jwtConfig)
        //{
        //    var builder = services.AddIdentity<ApplicationUser, IdentityRole>(o =>
        //    {
        //        o.Password.RequireDigit = true;
        //        o.Password.RequireLowercase = false;
        //        o.Password.RequireUppercase = false;
        //        o.Password.RequireNonAlphanumeric = false;
        //        o.Password.RequiredLength = 10;
        //        o.User.RequireUniqueEmail = true;
        //    })
        //    .AddEntityFrameworkStores<ApplicationDbContext>()
        //    .AddDefaultTokenProviders();

        //    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims

        //    services.AddAuthentication(options =>
        //    {
        //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    })
        //    .AddJwtBearer(options =>
        //    {
        //        options.RequireHttpsMetadata = false;
        //        options.SaveToken = true;
        //        var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.JwtKey));
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            IssuerSigningKey = serverSecret,
        //            ValidIssuer = jwtConfig.JwtIssuer,
        //            ValidAudience = jwtConfig.JwtAudience,
        //            ClockSkew = TimeSpan.Zero,
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidateLifetime = true,
        //            ValidateIssuerSigningKey = true,
        //        };
        //    });

        //    services.AddAuthorization();
        //}

        public static void ConfigureAuthentication(this IServiceCollection services, JwtConfig jwtConfig)
        {
            

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.JwtKey));
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = serverSecret,
                    ValidIssuer = jwtConfig.JwtIssuer,
                    ValidAudience = jwtConfig.JwtAudience,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };
            });

            services.AddAuthorization();
        }

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
           services.AddDbContext<ApplicationDbContext>(opts =>
           opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
    }
}

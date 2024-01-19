using Ems_API.Configurations;
using Ems_API.Extentions;
using Ems_Services.Helper;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
IServiceCollection services = builder.Services;

EmsApiConfig emsConfig = configuration.Get<EmsApiConfig>()!;
services.AddSingleton(emsConfig);
JwtConfig jwtConfig = emsConfig.JwtConfig;
services.AddSingleton(jwtConfig);

builder.Services.ConfigureSwagger();
// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureAuthentication(jwtConfig);

builder.Services.AddControllers();
builder.Services.RegisterServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowBlazorWasm", builder => builder
//    .WithOrigins("http://localhost:5130", "https://localhost:7272")
//    .AllowAnyMethod()
//    .AllowAnyHeader()
//    .AllowCredentials());
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazorWasm");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

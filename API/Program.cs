using API.Services;
using API.Utils.User;
using Domain.Configs;
using Domain.Repositories;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var securityScheme = new OpenApiSecurityScheme()
{
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JSON Web Token Based Security"
};

var securityRequirement = new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        },
        new string[] { }
    }
};

builder.Services.AddControllers();

var configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json");

IConfiguration config = configurationBuilder.Build();

var jwtSection = config.GetSection("JwtConfig");
JwtConfig.Key = jwtSection.GetValue(typeof(string), "Key").ToString();
JwtConfig.Audience = jwtSection.GetValue(typeof(string), "Audience").ToString();
JwtConfig.Issuer = jwtSection.GetValue(typeof(string), "Issuer").ToString();
JwtConfig.ValidInMinutes = Int32.Parse(jwtSection.GetValue(typeof(string), "ValidInMinutes").ToString());

var dbSection = config.GetSection("DbConfig");
DbConfig.Server = dbSection.GetValue(typeof(string), "Server").ToString();
DbConfig.Database = dbSection.GetValue(typeof(string), "Database").ToString();
DbConfig.UserId = dbSection.GetValue(typeof(string), "UserId").ToString();
DbConfig.Password = dbSection.GetValue(typeof(string), "Password").ToString();

var rabbitMqSection = config.GetSection("RabbitMqConfig");
RabbitMqConfig.HostName = rabbitMqSection.GetValue(typeof(string), "HostName").ToString();
RabbitMqConfig.UserName = rabbitMqSection.GetValue(typeof(string), "UserName").ToString();
RabbitMqConfig.Password = rabbitMqSection.GetValue(typeof(string), "Password").ToString();
RabbitMqConfig.VirtualHost = rabbitMqSection.GetValue(typeof(string), "VirtualHost").ToString();

builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddSwaggerGen(
        o =>
        {
            o.AddSecurityDefinition("Bearer", securityScheme);
            o.AddSecurityRequirement(securityRequirement);
        });

builder.Services.AddSingleton<IMessageProducer, MessageProducer>();
builder.Services.AddTransient<IUserUtils, UserUtils>();

builder.Services
    .AddAuthentication(
        o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(
        o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = JwtConfig.Issuer,
                ValidAudience = JwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
        });

builder.Services.AddTransient<IUserRepository, UserRepository>();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

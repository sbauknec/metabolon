using Microsoft.EntityFrameworkCore;

using metabolon.Models;
using metabolon.Generic;
using metabolon.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!);

//Setup, Authorisierung und Verifizierung via JSonWebToken
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

//Setup, Controller und SwaggerUI
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

//Setup, Datenbank Verbindung
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).LogTo(Console.WriteLine, LogLevel.Information));

//Setup, Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

//Jwt Session Tokens
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

//Konfiguration, Dev Logger und so weiter, auch Ports
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//Konfiguration, Swagger UI, Pipeline, Endpunkt
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", ":metabolon API V1");
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

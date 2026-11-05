
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieTicketBooking.Constants;
using MovieTicketBooking.Data;
using MovieTicketBooking.Exceptions;
using MovieTicketBooking.MIDDLEWARE;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services;
using MovieTicketBooking.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// CORS
// ======================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins(
            "http://localhost:53802",
            "http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// ======================================================
// JWT AUTHENTICATION CONFIGURATION
// ======================================================

var jwtSettings = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSettings["Key"]
    ?? throw new InvalidOperationException("JWT Key is missing.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)),

        ClockSkew = TimeSpan.Zero
    };
});

// ======================================================
// AUTHORIZATION
// ======================================================

builder.Services.AddAuthorization();

// ======================================================
// CONTROLLERS
// ======================================================

builder.Services.AddControllers();


// ======================================================
// BUILD APPLICATION
// ======================================================

var app = builder.Build();


// ======================================================
// 1. HTTPS REDIRECTION
// ======================================================

app.UseHttpsRedirection();


// ======================================================
// 2. CORS
// ======================================================

app.UseCors("AllowAngular");


// ======================================================
// 3. ENDPOINT METADATA DIAGNOSTICS
// ======================================================

if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<EndpointMetadataMiddleware>();
}


// ======================================================
// 4. AUTHENTICATION
// Validates JWT and populates HttpContext.User
// ======================================================

app.UseAuthentication();


// ======================================================
// 5. AUTHENTICATION & AUTHORIZATION DIAGNOSTICS
// ======================================================

if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<AuthenticationDiagnosticsMiddleware>();

    app.UseMiddleware<AuthorizationDiagnosticsMiddleware>();
}


// ======================================================
// 6. AUTHORIZATION
// Checks endpoint access permissions
// ======================================================

app.UseAuthorization();


// ======================================================
// 7. MAP CONTROLLERS
// ======================================================

app.MapControllers();


// ======================================================
// 8. RUN APPLICATION
// ======================================================

app.Run();

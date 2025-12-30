using CloudReady.API.Middleware;
using CloudReady.Application.Interfaces;
using CloudReady.Domain.Securities;
using CloudReady.Infrastructure.Persistence;
using CloudReady.Infrastructure.Security;
using CloudReady.Infrastructure.Tenancy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CloudReady API",
        Version = "v1",
        Description = "Multi-Tenant SaaS Backend API"
    });

    // 🔑 JWT Bearer
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT}"
    });

    // 🏢 Tenant Header
    options.AddSecurityDefinition("Tenant", new OpenApiSecurityScheme
    {
        Name = "X-Tenant-Code",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Tenant Code (example: acme)"
    });

    // 🔒 Apply both globally
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Tenant"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdmin", policy =>
        policy.RequireRole(Roles.Admin, Roles.Owner))
    .AddPolicy("RequireOwner", policy =>
        policy.RequireRole(Roles.Owner));

// Tenant services
builder.Services.AddScoped<TenantProvider>();
builder.Services.AddScoped<ITenantProvider>(sp =>
    sp.GetRequiredService<TenantProvider>());
builder.Services.AddHttpContextAccessor();

// PasswordHasher services
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// JwtTokenService services
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "swagger";
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CloudReady API v1");
    });
}

app.UseHttpsRedirection();

// Tenant middleware
app.UseMiddleware<TenantResolutionMiddleware>();

app.UseAuthentication();   // MUST be before authorization
app.UseAuthorization();

app.MapControllers();

app.Run();

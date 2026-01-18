using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserService.Application.Services;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Identity;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Repositories;
using UserService.Application.Interfaces;
using UserService.Application.BackgroundJobs;
using Stellar.Shared.Extensions;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// ======================
// Add services
// ======================

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddGrpc();

builder.Services.AddStellarSwagger("UserService.API");

// ======================
// DbContext
// ======================

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("UserDbConnection")
    )
);

// ======================
// Identity
// ======================

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Password policy
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<UserDbContext>()
.AddDefaultTokenProviders();

// Token lifespan (reset password, confirm email...)
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
{
    opt.TokenLifespan = TimeSpan.FromHours(3);
});

// ======================
// JWT Authentication
// ======================

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["JwtSettings:SecretKey"]!
            )
        )
    };
});

// ======================
// DI
// ======================

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService.Application.Services.UserService>();
builder.Services.AddScoped<IFunctionRepository, FunctionRepository>();
builder.Services.AddScoped<IFunctionService, FunctionService>();
builder.Services.AddScoped<IRelationRoleFunctionRepository, RelationRoleFunctionRepository>();
builder.Services.AddScoped<IRelationRoleFunctionService, RelationRoleFunctionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<IRelationPlanFunctionRepository, RelationPlanFunctionRepository>();
builder.Services.AddScoped<IUserPlanSubscriptionRepository, UserPlanSubscriptionRepository>();
builder.Services.AddScoped<IPlanService, PlanService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddHostedService<SubscriptionExpiryJob>();
builder.Services.AddStellarExcel();

// ======================
// Build app
// ======================

var app = builder.Build();

// ======================
// Middleware
// ======================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<UserService.API.Grpc.UserGrpcService>();

app.Run();

using CourseService.Infrastructure.Database;
using Stellar.Shared.Middlewares;
using Stellar.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CourseService.Domain.Services.Persistence;
using CourseService.Infrastructure.Persistence.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Configure DB
// Using SQL Server as requested for migrations
builder.Services.AddDbContext<StellarDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Shared Services
builder.Services.AddScoped<HeaderContext>();

// Register Application Services
builder.Services.AddScoped<CoursePersistence, CourseRepository>();
builder.Services.AddScoped<CourseService.Application.Usecases.CourseService>();

// Register Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Stellar Education API", Version = "v1" });
    // Add Security Definition for X-User if needed, or just let users pass it manually
    c.AddSecurityDefinition("X-User", new OpenApiSecurityScheme
    {
        Name = "X-User",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "JSON Web Token or JSON Object for User Context"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "X-User" }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configure the HTTP request pipeline.
// Enable Swagger in all environments for demo purposes
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<HeaderContextMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

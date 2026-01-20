using CourseService.Infrastructure.Database;
using Stellar.Shared.Middlewares;
using Stellar.Shared.Models;
using Microsoft.EntityFrameworkCore;
using CourseService.Domain.Services.Persistence;
using Stellar.Shared.Protos;
using Stellar.Shared.Extensions;
using CourseService.Infrastructure.Services.Repository;
using CourseService.Infrastructure.Services.Communication;

DotNetEnv.Env.Load();

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

// Register gRPC Client
builder.Services.AddGrpcClient<UserLookup.UserLookupClient>(o =>
{
    o.Address = new Uri(builder.Configuration["GrpcSettings:UserServiceUrl"] ?? "http://localhost:5001");
});
builder.Services.AddScoped<IUserGrpcClient, UserGrpcClient>();

// Register Swagger
builder.Services.AddStellarSwagger("CourseService.API");

var app = builder.Build();

// Configure the HTTP request pipeline.
// Configure the HTTP request pipeline.
// Enable Swagger in all environments for demo purposes
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<HeaderContextMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

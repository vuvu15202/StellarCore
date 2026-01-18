using Microsoft.EntityFrameworkCore;
using PaymentService.Application.Usecases;
using PaymentService.Application.Usecases.Strategies;
using PaymentService.Domain.Services.Persistence;
using PaymentService.Infrastructure.Database;
using PaymentService.Infrastructure.Services.Repository;
using Stellar.Shared.Middlewares;
using Stellar.Shared.Models;
using Stellar.Shared.Extensions;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(); // Using Stellar Shared Swagger if possible, or standard

// Configure DB
builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Shared Services
builder.Services.AddScoped<HeaderContext>();

// Register Repositories
builder.Services.AddScoped<PaymentPersistence, PaymentRepository>();
builder.Services.AddScoped<WalletPersistence, WalletRepository>();

// Register Strategies
builder.Services.AddScoped<IPaymentStrategy, VnPayPaymentStrategy>();
builder.Services.AddScoped<IPaymentStrategy, MomoPaymentStrategy>();
builder.Services.AddScoped<IPaymentStrategy, WalletPaymentStrategy>();

// Register Factory
builder.Services.AddScoped<IPaymentStrategyFactory, PaymentStrategyFactory>();

// Register Services
builder.Services.AddScoped<WalletService>();
builder.Services.AddScoped<PaymentService.Application.Usecases.PaymentService>();

// Configure Swagger with Stellar Shared logic
builder.Services.AddStellarSwagger("PaymentService.API"); 

// builder.Services.AddSwaggerGen(); // Fallback

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseMiddleware<HeaderContextMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

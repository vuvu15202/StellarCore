using MediaService.Domain.Services;
using MediaService.Domain.Services.Persistence;
using MediaService.Infrastructure.Data;
using MediaService.Infrastructure.Options;
using MediaService.Infrastructure.Services;
using MediaService.Infrastructure.Services.Repositories;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Stellar.Shared.Extensions;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddStellarSwagger("MediaService.API");

// Database
builder.Services.AddDbContext<MediaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services & Repositories
builder.Services.AddScoped<MediaPersistence, MediaRepository>();
builder.Services.AddScoped<IFileService, FileService>();

// Configure File Upload Limits
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100MB
});

builder.Services.Configure<MediaOptions>(
builder.Configuration.GetSection("Media"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();

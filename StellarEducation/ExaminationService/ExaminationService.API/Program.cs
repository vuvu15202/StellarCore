using ExaminationService.Application.Services;
using ExaminationService.Application.Factories;
using ExaminationService.Application.Strategies;
using ExaminationService.Domain.Interfaces;
using ExaminationService.Domain.Services.Persistence;
using ExaminationService.Infrastructure.Database;
using ExaminationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using ExaminationService.Application.Interfaces;
using Stellar.Shared.Extensions;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StellarDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("UserDbConnection")
    )
);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddStellarSwagger("ExaminationService.API");


// Skill Services
builder.Services.AddScoped<IExamService, ReadingExamService>();
builder.Services.AddScoped<IExamService, ListeningExamService>();
builder.Services.AddScoped<IExamService, WritingExamService>();
builder.Services.AddScoped<IExamService, SpeakingExamService>();

// ExamType Strategies
builder.Services.AddScoped<IExamTypeStrategy, TestStrategy>();
builder.Services.AddScoped<IExamTypeStrategy, HomeworkStrategy>();

// Factories
builder.Services.AddScoped<IExamServiceFactory, ExamServiceFactory>();
builder.Services.AddScoped<IExamTypeStrategyFactory, ExamTypeStrategyFactory>();



// repo
builder.Services.AddScoped<ExamAttemptPersistence, ExamAttemptRepository>();
builder.Services.AddScoped<IExamService1, ExamService>();
builder.Services.AddScoped<IExamAttemptService, ExamAttemptService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

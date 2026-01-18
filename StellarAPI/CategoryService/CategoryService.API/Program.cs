using CategoryService.Application.Interfaces;
using CategoryService.Application.Mappings;
using CategoryService.Application.Services;
using CategoryService.Domain.Entities;
using CategoryService.Domain.Repositories;
using CategoryService.Infrastructure.Persistence;
using CategoryService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Stellar.Shared.Extensions;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddStellarSwagger("CategoryService.API");

// DbContext
builder.Services.AddDbContext<CategoryDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("CategoryDbConnection")
    )
);

// AutoMapper
builder.Services.AddAutoMapper(typeof(CategoryMappingProfile));

// Repositories
builder.Services.AddScoped<ICategoryTypeRepository, CategoryTypeRepository>();
// Registration for specific categories
builder.Services.AddScoped<IBaseCategoryRepository<CommonCategory>, BaseCategoryRepository<CommonCategory>>();
builder.Services.AddScoped<IBaseCategoryRepository<BloodTypeCategory>, BaseCategoryRepository<BloodTypeCategory>>();
builder.Services.AddScoped<IBaseCategoryRepository<EthnicCategory>, BaseCategoryRepository<EthnicCategory>>();
builder.Services.AddScoped<IBaseCategoryRepository<MaritalStatusCategory>, BaseCategoryRepository<MaritalStatusCategory>>();
builder.Services.AddScoped<IBaseCategoryRepository<NationCategory>, BaseCategoryRepository<NationCategory>>();
builder.Services.AddScoped<IBaseCategoryRepository<RelationShipCategory>, BaseCategoryRepository<RelationShipCategory>>();
builder.Services.AddScoped<IBaseCategoryRepository<ReligionCategory>, BaseCategoryRepository<ReligionCategory>>();
builder.Services.AddScoped<IBaseCategoryRepository<SexualCategory>, BaseCategoryRepository<SexualCategory>>();
builder.Services.AddScoped<IBaseCategoryRepository<FaqCategory>, BaseCategoryRepository<FaqCategory>>();
builder.Services.AddScoped<IBaseCategoryRepository<UserManualCategory>, BaseCategoryRepository<UserManualCategory>>();
builder.Services.AddScoped<IBaseCategoryRepository<ProvinceCategory>, BaseCategoryRepository<ProvinceCategory>>();
builder.Services.AddScoped<IBaseCategoryRepository<DistrictCategory>, BaseCategoryRepository<DistrictCategory>>();
builder.Services.AddScoped<IBaseCategoryRepository<WardCategory>, BaseCategoryRepository<WardCategory>>();

// Services
builder.Services.AddScoped<ICategoryRepositoryProvider, CategoryRepositoryProvider>();
builder.Services.AddScoped<ICategoryTypeService, CategoryTypeService>();
builder.Services.AddScoped<ICategoryService, CategoryService.Application.Services.CategoryService>();

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

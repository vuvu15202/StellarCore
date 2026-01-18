using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using Stellar.Shared.Extensions;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// ======================
// Load Ocelot config
// ======================

builder.Configuration.AddJsonFile(
    "ocelot.json",
    optional: false,
    reloadOnChange: true
);

// ======================
// Register Ocelot
// ======================

builder.Services.AddOcelot(builder.Configuration);

// ======================
// Register Swagger
// ======================
builder.Services.AddStellarSwagger("APIGateway");

// ======================
// Build app
// ======================

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ======================
// Ocelot middleware
// MUST be last
// ======================

await app.UseOcelot();

app.Run();

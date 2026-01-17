using Ocelot.Middleware;
using Ocelot.DependencyInjection;

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
// Build app
// ======================

var app = builder.Build();

app.UseHttpsRedirection();

// ======================
// Ocelot middleware
// MUST be last
// ======================

await app.UseOcelot();

app.Run();

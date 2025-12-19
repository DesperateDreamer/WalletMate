using WalletMate.Adapters.In.API.Configurations;
using WalletMate.Adapters.In.API.Extensions;
using WalletMate.Adapters.In.API.Middleware;
using WalletMate.Adapters.Out.Cache;
using WalletMate.Adapters.Out.Database;
using WalletMate.Adapters.Out.Monobank;
using WalletMate.Adapters.Out.Shared.Http;
using WalletMate.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.ConfigureSwagger();
builder.ConfigureJwtAuthentication();
builder.ConfigureAuthorization();
builder.ConfigureDbContext();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSecurityAdapters();
builder.Services.ConfigureApplicationAdapters();
builder.Services.ConfigureDatabaseAdapters();
builder.Services.ConfigureCacheAdapters();
builder.Services
    .ConfigureSharedHttpAdapters()
    .ConfigureMonobankAdapters();

var app = builder.Build();
await app.RunMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
using WalletMate.Adapters.In.API.Configurations;
using WalletMate.Adapters.In.API.Extensions;
using WalletMate.Adapters.In.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.ConfigureSwagger();
builder.ConfigureJwtAuthentication();
builder.ConfigureAuthorization();
builder.ConfigureDbContext();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
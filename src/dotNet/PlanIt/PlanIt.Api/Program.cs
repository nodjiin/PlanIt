using PlanIt.Persistence;
using PlanIt.Persistence.Mocked;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

// Add services to the container.

if (isDevelopment)
{
    builder.Services.AddMockedPersistenceServices();
}

if (isProduction)
{
    builder.Services.AddPersistenceServices(configuration);
}


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

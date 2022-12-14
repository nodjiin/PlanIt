using PlanIt.Application;
using PlanIt.Persistence;
using PlanIt.Persistence.Mocked;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var isMockedPersistenceEnabled = configuration.GetSection("MockedPersistence").Value == "enabled";
if (isMockedPersistenceEnabled) builder.Services.AddMockedPersistenceServices();
else builder.Services.AddPersistenceServices(configuration);

builder.Services.AddApplicationServices(configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

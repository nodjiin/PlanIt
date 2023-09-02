using PlanIt.Application;
using PlanIt.Persistence;
using PlanIt.Persistence.Mocked;

const string CPolicy = "CORSPOLICY";

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var isMockedPersistenceEnabled = configuration.GetSection("MockedPersistence").Value == "enabled";
if (isMockedPersistenceEnabled) builder.Services.AddMockedPersistenceServices();
else builder.Services.AddPersistenceServices(configuration);

builder.Services.AddApplicationServices(configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CPolicy,
                      policy =>
                      {
                          policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(CPolicy);
app.UseAuthorization();

app.MapControllers();

app.Run();

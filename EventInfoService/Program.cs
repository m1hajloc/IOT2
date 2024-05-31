using EventInfoService.Services;
using EventInfoService.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using EventInfoService.Services;
using EventInfoService.Controllers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
 // Dodaj MqttService kao Hosted Service

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EventInfoService", Version = "v1" });
});

builder.Services.AddControllers();
builder.Services.AddSingleton<MqttService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventInfoService v1"));
}

app.UseHttpsRedirection();

// Dodajte autorizaciju ako je potrebno
app.UseAuthorization();

// Mapirajte kontrolere u HTTP request pipeline
app.MapControllers();

app.Run();

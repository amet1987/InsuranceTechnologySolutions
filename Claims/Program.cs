using System.Text.Json.Serialization;
using Application.Extensions;
using Claims.Middlewars;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Add Application Services
builder.Services.AddApplicationServices(builder.Configuration);


// Add Exception Handler
builder.Services.AddExceptionHandler<ExceptionHandlingMiddleware>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Claim API",
        Version = "v1",
        Description = "Claim API for demo purpose"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(_ => { });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Apply DB Migration
app.ApplyMigrations();

app.Run();

public partial class Program { }

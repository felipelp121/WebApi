using WebApi.Data;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

ConfigureDatabase.ConfigureDatabaseServices(builder.Services, builder.Configuration);

builder.Services.AddCustomServices();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.MapControllers();

app.Run();


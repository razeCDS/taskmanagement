using Serilog;
using TaskManagement.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddLoggerConfiguration();
builder.AddApplicationServices();
builder.AddInfraStructureServices();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

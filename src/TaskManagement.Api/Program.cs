using TaskManagement.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();
builder.AddInfraStructureServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

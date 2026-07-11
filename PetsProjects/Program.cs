using Microsoft.EntityFrameworkCore;
using PetsProjects.DataBaseAccess.Postgres;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<PetsProjectDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(PetsProjectDbContext)));
    });

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

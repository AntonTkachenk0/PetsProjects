using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetsProjects.Abstractions;
using PetsProjects.Controllers.Contracts;
using PetsProjects.Controllers.Contracts.Validator;
using PetsProjects.DataBaseAccess.Postgres;
using PetsProjects.DataBaseAccess.Postgres.Repositories;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IPetRepository, PetRepository>();
builder.Services.AddTransient<IServiceRepository, ServiceRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<IValidator<PetRequest>, PetRequestValidator>();
builder.Services.AddTransient<IValidator<ServiceRequest>, ServiceRequestValidator>();

builder.Services.AddDbContext<PetsProjectDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString(nameof(PetsProjectDbContext)));
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});
var app = builder.Build();

app.UseCors();
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

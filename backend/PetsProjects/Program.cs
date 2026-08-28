using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using PetsProjects.Abstractions;
using PetsProjects.Controllers.Contracts;
using PetsProjects.Controllers.Contracts.Validator;
using PetsProjects.DataBaseAccess.Postgres;
using PetsProjects.DataBaseAccess.Postgres.Repositories;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


Env.Load();


builder.Configuration.AddEnvironmentVariables();



builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddTransient<IPetRepository, PetRepository>();
builder.Services.AddTransient<IServiceRepository, ServiceRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IServiceJournalRepository, ServiceJournalRepository>();

builder.Services.AddTransient<IValidator<PetRequest>, PetRequestValidator>();
builder.Services.AddTransient<IValidator<ServiceRequest>, ServiceRequestValidator>();
builder.Services.AddTransient<IValidator<FileUploadDto>, FileRequestValidator>();
builder.Services.AddTransient<IValidator<ServiceJournalRequest>, ServiceJournalRequestValidator>();

builder.Services.Configure<S3Options>(options =>
{
    options.AccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY") ?? "";
    options.SecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") ?? "";
    options.ServiceUrl = Environment.GetEnvironmentVariable("AWS_SERVICE_URL") ?? "";
    options.BucketName = Environment.GetEnvironmentVariable("AWS_BUCKET_NAME") ?? "";
});
builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var options = sp.GetRequiredService<IOptions<S3Options>>().Value;
    return new AmazonS3Client(options.AccessKey, options.SecretKey, new AmazonS3Config
    {
        ServiceURL = options.ServiceUrl,
        ForcePathStyle = true
    });
});

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

builder.Services.AddSwaggerGen(c =>
{
    c.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = JsonSchemaType.String,  
        Format = "binary"
    });
});

var app = builder.Build();


app.MapScalarApiReference();

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

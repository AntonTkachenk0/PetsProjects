using FluentValidation;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using PetsProjects;
using PetsProjects.Abstractions;
using PetsProjects.Controllers.Contracts;
using PetsProjects.Controllers.Contracts.Validator;
using PetsProjects.DataBaseAccess.Postgres;
using PetsProjects.DataBaseAccess.Postgres.Repositories;
using PetsProjects.Extensions;
using PetsProjects.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


Env.Load();


builder.Configuration.AddEnvironmentVariables();



builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
builder.Services.AddApiAuthentication(Options.Create(jwtOptions));

builder.Services.AddTransient<UsersService>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IServiceJournalRepository, ServiceJournalRepository>();

builder.Services.AddTransient<IJwtProvider, JwtProvider>();
builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();

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
        policy.WithOrigins(
            "http://localhost:5173",
            "http://localhost:5067",  
            "https://localhost:7241"   
        );
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
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

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
});


app.MapControllers();

app.Run();

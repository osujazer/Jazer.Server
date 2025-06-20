using System.Text;
using Asp.Versioning;
using FluentValidation;
using Jazer.Server.Config;
using Jazer.Server.Cryptography;
using Jazer.Server.Database;
using Jazer.Server.Handlers;
using Jazer.Server.Repositories;
using Jazer.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddOptions<Settings>()
    .Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

var settings = builder.Configuration.Get<Settings>()!;

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApi();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.MapInboundClaims = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtSecretKey)),
            ValidIssuer = settings.JwtIssuer,
            ValidAudience = settings.JwtAudience,
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddDbContext<JazerDbContext>(options =>
    options.UseNpgsql(settings.DatabaseConnectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionHandler();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<JazerDbContext>();
    context!.Database.Migrate();
}

app.Run($"http://{settings.ServiceHost}:{settings.ServicePort}");

// NOTE: this allows tests to access the assembly
public partial class Program;
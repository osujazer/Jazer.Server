using System.Text;
using Asp.Versioning;
using FluentValidation;
using Genbox.SimpleS3.Core.Common.Authentication;
using Genbox.SimpleS3.Extensions.Wasabi;
using Genbox.SimpleS3.Wasabi.Extensions;
using Jazer.Server.Authentication;
using Jazer.Server.Config;
using Jazer.Server.Cryptography;
using Jazer.Server.Database;
using Jazer.Server.Extensions;
using Jazer.Server.Handlers;
using Jazer.Server.Repositories;
using Jazer.Server.Services;
using Mapster;
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
builder.Services.AddMapster();
builder.Services.RegisterMapsterConfiguration();
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

builder.Services.AddWasabi(config =>
{
    config.Region = WasabiRegion.EuCentral2;
    config.Credentials = new StringAccessKey(settings.S3AccessKey, settings.S3SecretKey);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUserStatisticsRepository, UserStatisticsRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStorageService, WasabiStorageService>();
builder.Services.AddScoped<IAvatarService, AvatarService>();

builder.Services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
builder.Services.AddSingleton<TokenProvider>();

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
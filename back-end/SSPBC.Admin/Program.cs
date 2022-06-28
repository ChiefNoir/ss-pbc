using Abstractions.Cache;
using Abstractions.RepositoryPrivate;
using Abstractions.Security;
using Infrastructure;
using Infrastructure.Cache;
using Infrastructure.RepositoriesPrivate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Security;
using StackExchange.Redis;

const string KeyDatabase = "PostgreSQL";
const string KeyCache = "Redis";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors();

#if DEBUG
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
#endif

builder.Services.AddApiVersioning(o =>
{
    o.ReportApiVersions = true;
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(KeyDatabase));
});
var options = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString(KeyCache));
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options));
builder.Services.AddTransient<IDataCache, DataCache>();
builder.Services.AddTransient<IPrivateCategoryRepository, PrivateCategoryRepository>();
builder.Services.AddTransient<IPrivateProjectRepository, PrivateProjectRepository>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IPrivateIntroductionRepository, PrivateIntroductionRepository>();
builder.Services.AddTransient<IFileRepository, FileRepository>();
builder.Services.AddTransient<ISessionRepository, SessionRepository>();
builder.Services.AddTransient<ITokenManager, TokenManager>();
builder.Services.AddTransient<Supervisor>();
builder.Services.AddTransient<HashManager>();

builder.Services
       .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.RequireHttpsMetadata = builder.Configuration.GetSection("Token:RequireHttpsMetadata").Get<bool>();
           options.TokenValidationParameters = TokenManager.CreateTokenValidationParameters(builder.Configuration);
       });

//MultiPartBodyLength
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader());

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var sessionRepository = services.GetRequiredService<ISessionRepository>();
    await sessionRepository.FlushAsync();
}

app.Run();

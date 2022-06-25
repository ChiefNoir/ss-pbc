using Abstractions.Cache;
using Abstractions.IRepositories;
using Abstractions.Security;
using Infrastructure;
using Infrastructure.Cache;
using Infrastructure.Repositories;
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

#if DEBUG
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endif

builder.Services.AddCors();

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
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IIntroductionRepository, IntroductionRepository>();
builder.Services.AddTransient<IFileRepository, FileRepository>();
builder.Services.AddTransient<ITokenManager, TokenManager>();
builder.Services.AddTransient<IDataCache, DataCache>();
builder.Services.AddTransient<Supervisor>();
builder.Services.AddTransient<HashManager>();


var options = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString(KeyCache));

var multiplexer = ConnectionMultiplexer.Connect(options);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

    var context = services.GetRequiredService<DataContext>();
    context.Migrator.MigrateUp();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<IDataCache>();
    await context.FlushAsync();
}

app.Run();

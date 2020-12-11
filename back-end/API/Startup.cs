using Abstractions.IRepository;
using Abstractions.ISecurity;
using Abstractions.Supervision;
using Infrastructure;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using Security;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;

namespace API
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddControllers();
            services.AddCors();

            services.AddDbContext<DataContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Default")));
            
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IIntroductionRepository, IntroductionRepository>();
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IHashManager, HashManager>();
            services.AddTransient<ISupervisor, Supervisor>();
            services.AddTransient<ITokenManager, TokenManager>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = Configuration.GetSection("Token:RequireHttpsMetadata").Get<bool>();
                    options.TokenValidationParameters = TokenManager.CreateTokenValidationParameters(Configuration);
                });

            //MultiPartBodyLength
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddLogging(builder =>
            {
                builder.AddNLog("nlog.config");
            });
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseRouting();
            app.UseCors
            (
                options => options.SetIsOriginAllowed(x => _ = true)
                                  .AllowAnyMethod()
                                  .AllowAnyHeader()
                                  .AllowCredentials()
            );


            var path = configuration.GetSection("Location:FileStorage").Get<string>();
            CheckFileStorageDirectory(path);

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = new PathString("/" + path)
            });


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void CheckFileStorageDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
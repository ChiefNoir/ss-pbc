using Abstractions.IRepository;
using Abstractions.ISecurity;
using Abstractions.MemoryCache;
using Abstractions.Model;
using API.Security;
using Infrastructure;
using Infrastructure.Cache;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Secutiry;

namespace BusinessService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

 
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();

            services.AddDbContext<DataContext>(options => options.UseNpgsql(Configuration.GetConnectionString("Default")), ServiceLifetime.Scoped);
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<INewsRepository, NewsRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IHashManager, HashManager>();

            services.AddSingleton<IMemoryCache<string, Category>, CategoryCache>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = TokenManager.CreateTokenValidationParameters(Configuration);
                    });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors
                (
                    options => options.SetIsOriginAllowed(x => _ = true)
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials()
                );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        
    }
}

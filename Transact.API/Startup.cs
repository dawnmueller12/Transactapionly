using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using Transact.API.Extensions;
using Transact.Data.Abstractions.DBFactory;
using Transact.Data.Abstractions.Repository;
using Transact.Data.Abstractions.Services;
using Transact.Data.Abstractions.UnitOfWork;
using Transact.Data.Core;
using Transact.Data.Models;
using Transact.Data.Models.Common;
using Transact.Data.Repository;
using Transact.Data.Services;

namespace Transact
{
    public class Startup
    {

        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            // Register SQL database configuration context as services.
            services.AddDbContext<TransactDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TransactDBContext"));
            });

            services.AddScoped(typeof(IEntityBaseRepository<>), typeof(EntityBaseRepository<>));
            services.AddScoped(typeof(IDbFactory), typeof(DbFactory));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IDataRepositoryFactory), typeof(DataRepositoryFactory));

            services.AddMvc()
                .AddNewtonsoftJson(opt =>
                {
                    var settings = opt.SerializerSettings;
                    settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    var resolver = opt.SerializerSettings.ContractResolver as DefaultContractResolver;
                    resolver.NamingStrategy = null;
                });

            services.ConfigureBusinessServices(Configuration, "");

            services.AddSwaggerGen(action =>
            {

                action.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Transact API",
                    Version = "v1"
                });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                action.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

                action.AddSecurityRequirement(securityRequirement);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddMicrosoftIdentityWebApi(options =>
                   {
                       Configuration.Bind("AppSettings:AzureADB2CConfig", options);
                       options.TokenValidationParameters.NameClaimType = "name";
                   },
                   options =>
                   {
                       Configuration.Bind("AppSettings:AzureADB2CConfig", options);
                   });

            // Automapper settings
            services.ConfigureMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transact v1"));

            app.UseRouting();

            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

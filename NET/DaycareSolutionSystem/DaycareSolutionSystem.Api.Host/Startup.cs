using System.Collections.Generic;
using System.Text;
using DaycareSolutionSystem.Api.Host.DatabaseValidation;
using DaycareSolutionSystem.Api.Host.Services.Actions;
using DaycareSolutionSystem.Api.Host.Services.AgreedActions;
using DaycareSolutionSystem.Api.Host.Services.Authentication;
using DaycareSolutionSystem.Api.Host.Services.Clients;
using DaycareSolutionSystem.Api.Host.Services.Employees;
using DaycareSolutionSystem.Api.Host.Services.IndividualPlans;
using DaycareSolutionSystem.Api.Host.Services.RegisteredActions;
using DaycareSolutionSystem.Database.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DaycareSolutionSystem.Api.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IConfiguration>(Configuration.GetSection("AppConfiguration"));

            services.AddDbContext<DssDataContext>(options =>
                options.UseNpgsql(Configuration.GetValue<string>("DssConnectionString")));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IRuntimeDatabaseValidator, RuntimeDatabaseValidator>();

            services.AddScoped<IJwtAuthenticationApiService, JwtAuthenticationApiService>();
            services.AddScoped<IRegisteredActionsApiService, RegisteredActionsApiService>();
            services.AddScoped<IClientApiService, ClientApiService>();
            services.AddScoped<IEmployeeApiService, EmployeeApiService>();
            services.AddScoped<IActionsApiService, ActionsApiService>();
            services.AddScoped<IIndividualPlansApiService, IndividualPlansApiService>();
            services.AddScoped<IAgreedActionsApiService, AgreedActionsApiService>();


            var securityKey = Configuration.GetSection("AppConfiguration")?.GetValue<string>("SecurityKey");

            // Configure authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "DayCareSolutionSystemMobileApp",
                    ValidIssuer = "DayCareSolutionSystem",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))
                };
            });

            // Swagger
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }); });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}

using FlightMicroservice.DbContextFlight;
using FlightMicroservice.IJwtAuthentication;
using FlightMicroservice.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightMicroservice
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
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                   builder =>
                   {
                       builder.WithOrigins("http://localhost:5001").AllowAnyHeader().AllowAnyMethod();
                   });
            });
            services.AddDbContext<FlightContext>(o => o.UseSqlServer(Configuration.GetConnectionString("FlightMicroserviceDB")));
            services.AddTransient<IFlightRepository, FlightRepository>();
            services.AddTransient<IDiscountRepository, DiscountRepository>();
            services.AddSingleton<IJwtAuthenticationManager>(new JwtAuthenticationManager(Configuration["Jwt:Key"]));
            services.AddSwaggerGen(options =>
            {

                options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Name = "Authorize",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {

                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type =ReferenceType.SecurityScheme,Id="Bearer"
                            }
                        },
                        new string[]{}

                    }
                });

            });
            services.AddSingleton<QueueConsumer>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseCors(builder =>
            { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }
              );
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlightMicroservice V1"));
            app.UseAuthentication();
           app.UseQueueConsumer();
            //app.UseAuthorization();
            app.UseMvc();
        }
    }
    public static class ApplicationBuilderExtensions
    {
        private static QueueConsumer _consumer { get; set; }

        public static IApplicationBuilder UseQueueConsumer(this IApplicationBuilder app)
        {
            _consumer = app.ApplicationServices.GetService<QueueConsumer>();
            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();
            lifetime.ApplicationStarted.Register(OnStarted);

            lifetime.ApplicationStopping.Register(OnStopping);
            return app;

        }

        private static void OnStarted()
        {
            _consumer.Consume();
        }
        private static void OnStopping()
        {
            _consumer.Deregister();
        }

    }
}

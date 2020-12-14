using LikeButton.API.Filters;
using LikeButton.Core.Interfaces;
using LikeButton.Infrastructure.Logger;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeButton.API.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static void ResolveAPIFilters(this IServiceCollection services)
        {
            services.AddScoped<ModelStateValidationFilter>();
            services.AddScoped<AuthSecretKeyFilter>();
        }


        public static void ResolveCoreServices(this IServiceCollection services)
        {



        }

        public static void ResolveAPICors(this IServiceCollection services, IConfiguration config)
        {



            services.AddCors(options => ConfigureCorsPolicy(options));

            CorsOptions ConfigureCorsPolicy(CorsOptions corsOptions)
            {
                string allowedHosts = config["Appsettings:AllowedHosts"];

                if (string.IsNullOrEmpty(allowedHosts))
                    corsOptions.AddPolicy("DenyAllHost",
                                      corsPolicyBuilder => corsPolicyBuilder
                                      .AllowAnyHeader()
                                      .WithMethods(new string[4] { "POST", "PATCH", "HEAD", "OPTIONS" })
                                     );

                allowedHosts = allowedHosts.Trim();

                if (allowedHosts == "*")
                    corsOptions.AddPolicy("AllowAll",
                                     corsPolicyBuilder => corsPolicyBuilder
                                     .AllowAnyHeader()
                                     .AllowAnyMethod()
                                     .AllowAnyOrigin()
                                    );
                else
                {
                    string[] allowedHostArray;

                    if (!allowedHosts.Contains(","))
                        allowedHostArray = new string[1] { allowedHosts };
                    else
                        allowedHostArray = allowedHosts.Split(",", StringSplitOptions.RemoveEmptyEntries);

                    corsOptions.AddPolicy("AllowSpecificHost",
                                      corsPolicyBuilder => corsPolicyBuilder
                                      .AllowAnyHeader()
                                      .WithOrigins(allowedHostArray)
                                      .WithMethods(new string[4] { "POST", "PATCH", "HEAD", "OPTIONS" })
                                     );
                }

                return corsOptions;
            }
        }

        public static void ResolveSwagger(this IServiceCollection services, IConfiguration config)
        {
            bool.TryParse(config["Appsettings:EnableSwagger"], out bool enableSwagger);

            if (enableSwagger)
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger API for LikeButton", Version = "v1" });
                });
        }

    }
}

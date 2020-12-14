﻿using LikeButton.API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeButton.API.Extensions
{
    public static class IApplicationBuilderExtension
    {
        public static IApplicationBuilder UseCustomResponseHeaderMiddleware(this IApplicationBuilder appBuilder,
                        IDictionary<string, string> customHeaderMap = default)
        {
            return appBuilder.UseMiddleware<CustomResponseHeaderMiddleware>(customHeaderMap);
        }


        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder appBuilder, IConfiguration config)
        {
            bool.TryParse(config["Appsettings:EnableSwagger"], out bool enableSwagger);

            if (enableSwagger)
            {
                appBuilder.UseSwagger();

                appBuilder.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("./swagger/v1/swagger.json", "LikeButton V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            return appBuilder;
        }

        public static IApplicationBuilder ConfigureCors(this IApplicationBuilder appBuilder, IConfiguration config)
        {
            string allowedHosts = config["Appsettings:AllowedHosts"];

            if (string.IsNullOrEmpty(allowedHosts))
                return appBuilder.UseCors(x => x
                              .AllowAnyHeader()
                              .WithMethods(new string[4] { "POST", "PATCH", "HEAD", "OPTIONS" }));

            allowedHosts = allowedHosts.Trim();

            if (allowedHosts == "*")
                return appBuilder.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            else
            {
                string[] allowedHostArray;

                if (!allowedHosts.Contains(","))
                    allowedHostArray = new string[1] { allowedHosts };
                else
                    allowedHostArray = allowedHosts.Split(",", StringSplitOptions.RemoveEmptyEntries);

                return appBuilder.UseCors(x => x
                          .AllowAnyHeader()
                          .WithOrigins(allowedHostArray)
                          .WithMethods(new string[4] { "POST", "PATCH", "HEAD", "OPTIONS" }));
            }
        }
    }
}

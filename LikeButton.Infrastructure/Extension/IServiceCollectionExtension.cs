using LikeButton.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using LikeButton.Core.Interfaces;
using LikeButton.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using LikeButton.Infrastructure.Logger;
using LikeButton.Infrastructure.Data.Repository;
using LikeButton.Core.Interfaces.IAuthenticationRepository;
using LikeButton.Core.Interfaces.ILikeRepository;
using LikeButton.Infrastructure.Data.Repository.LikeRepository;
using LikeButton.Infrastructure.Data.Repository.ArticleRepository;
using LikeButton.Core.Interfaces.IUserRepository;
using LikeButton.Infrastructure.Data.Repository.UserRepository;
using LikeButton.Infrastructure.Helpers;

namespace LikeButton.Infrastructure
{
    public static class IServiceCollectionExtension
    {
        public static void ConfigureDBContextPool(this IServiceCollection services,IConfiguration config)
        {
            services.AddDbContextPool<AppDbContext>(optionBuilder =>
            {
                optionBuilder.UseSqlite(config.GetConnectionString("DefaultConnection"));
                optionBuilder.EnableSensitiveDataLogging();
            });
        }

        public static void ConfigureDBContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(optionBuilder =>
            {
                optionBuilder.UseSqlite(config.GetConnectionString("DefaultConnection"));
 
            });
        }


        public static void ResolveInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.ConfigureDBContextPool(config);
            services.AddScoped<IAuthentication, AuthenticationRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<IFileLogger, AppLoggerService>();

            services.AddHttpClient("BypassCertificateHttpClient").ConfigurePrimaryHttpMessageHandler(() =>
            {
                var clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                };
                return clientHandler;
            });
            HttpClientFactoryServiceCollectionExtensions.AddHttpClient(services);

        }

        public static void ConfigureTokenServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddScoped<IClaim, Claime>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(Options => {
               Options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                   .GetBytes(configuration
                   .GetSection("AppSettings:Token").Value)),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });
        }
    }
}

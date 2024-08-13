using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.CrossCuttingConcerns.Caching.Redis;
using Core.Utilities.IoC;
using Core.Utilities.Message.Abstract;
using Core.Utilities.Message.Concrete;
using Core.Utilities.Security.Abstract;
using Core.Utilities.Security.Concrete;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Core.DependencyResolver
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            // Dotnet Cache System
            services.AddMemoryCache();
            
            #region Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:1453"; // Your Redis configuration
            });
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:1453"));
            #endregion

            //services.AddHttpClient();
            services.AddTransient<ICacheManager, RedisChacheManager>();
            services.AddSingleton<ISmsService, SmsManager>();
            services.AddSingleton<IEmailService, EmailManager>();
            services.AddTransient<ITokenService, TokenManager>();
        }
    }
}

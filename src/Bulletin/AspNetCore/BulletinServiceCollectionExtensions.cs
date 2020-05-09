using System;
using Microsoft.Extensions.DependencyInjection;

namespace Bulletin.AspNetCore
{
    public static class BulletinServiceCollectionExtensions
    {
        public static IServiceCollection AddBulletin(
            this IServiceCollection services,
            Action<BulletinOptionsBuilder> optionsAction)
        {
            var options = new BulletinOptionsBuilder();
            optionsAction?.Invoke(options);

            services.AddScoped<IBulletin>(sp => options.Create());
            return services;
        }
    }
}
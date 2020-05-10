using System;
using Bulletin.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bulletin.AspNetCore
{
    public static class BulletinServiceCollectionExtensions
    {
        public static IServiceCollection AddBulletin<TContext>(
            this IServiceCollection services,
            Action<BulletinOptionsBuilder> optionsAction) where TContext : IBulletinDbContext
        {
            var optionsBuilder = new BulletinOptionsBuilder();
            optionsAction?.Invoke(optionsBuilder);

            var options = optionsBuilder.Create();

            services.AddScoped<IBulletin>(sp => new Bulletin(
                sp.GetRequiredService<IBulletinDbContext>(),
                options
            ));
            return services;
        }
    }
}
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
            Action<BulletinOptionsBuilder> optionsAction) where TContext : DbContext, IBulletinDbContext
        {
            var optionsBuilder = new BulletinOptionsBuilder();
            optionsAction?.Invoke(optionsBuilder);

            var options = optionsBuilder.Create();

            services.AddScoped<IBulletinDbContext>(sp => sp.GetService<TContext>());
            services.AddScoped<IBulletin>(sp => new Bulletin<TContext>(
                sp.GetRequiredService<TContext>(),
                options
            ));
            return services;
        }
    }
}
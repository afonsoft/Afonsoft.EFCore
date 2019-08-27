using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Afonsoft.EFCore
{
    public static class AfonsoftEFExtensions
    {
        /// <summary>
        /// AddAfonsoftRepository after add AddDbContext
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAfonsoftRepository(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }

        public static IServiceCollection AddAfonsoftRepository<TContext>(this IServiceCollection services, Action<AfonsoftEFOptions> optionsAction = null) where TContext : AfonsoftDbContext
        {
            
            if (optionsAction != null)
            {
                var opt = AfonsoftDbContext.BuildOptions(optionsAction);
                services.AddSingleton<AfonsoftEFOptions>(opt);
                services.AddSingleton<DbContextOptions<AfonsoftDbContext>>(opt.Options);
                services.AddSingleton<DbContextOptions>(opt.Options);
                services.AddDbContext<TContext>();
            }

            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }
    }
}

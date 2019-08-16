using Microsoft.EntityFrameworkCore;
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

        public static IServiceCollection AddAfonsoftRepository<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null) where TContext : AfonsoftDbContext
        {
            services.AddDbContext<TContext>(optionsAction);
            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }
    }
}

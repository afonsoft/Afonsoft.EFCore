using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Reflection;

namespace Afonsoft.EFCore
{
    /// <summary>
    /// RepositoryDbContext is a base of DbContext
    /// </summary>
    public class AfonsoftDbContext : DbContext
    {

        private static DbContextOptions<AfonsoftDbContext> GetOptions(EnumProvider provider, string connectionString = null, DbContextOptions<AfonsoftDbContext> dbContextOptions = null)
        {
            if (string.IsNullOrEmpty(connectionString) && dbContextOptions != null || (provider == EnumProvider.Unknown && dbContextOptions != null))
                return dbContextOptions;
            else
            {
                if (string.IsNullOrEmpty(connectionString) && provider == EnumProvider.SQLite)
                    connectionString = $"Data Source={Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "SQLite.db")}"; 

                if (string.IsNullOrEmpty(connectionString) && (provider == EnumProvider.InMemory || provider == EnumProvider.Unknown))
                    connectionString = "InMemoryDataBase";

                if (string.IsNullOrEmpty(connectionString))
                    throw new ArgumentNullException(nameof(connectionString), "Não existe uma conexão.");

                DbContextOptions<AfonsoftDbContext> _dbContextOptions;
                switch (provider)
                {
                    case EnumProvider.MySQL:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<AfonsoftDbContext>(dbContextOptions).UseMySql<AfonsoftDbContext>(connectionString).Options : new DbContextOptionsBuilder<AfonsoftDbContext>().UseMySql<AfonsoftDbContext>(connectionString).Options;
                        break;
                    case EnumProvider.SQLServer:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<AfonsoftDbContext>(dbContextOptions).UseSqlServer<AfonsoftDbContext>(connectionString).Options : new DbContextOptionsBuilder<AfonsoftDbContext>().UseSqlServer<AfonsoftDbContext>(connectionString).Options;

                        break;
                    case EnumProvider.SQLite:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<AfonsoftDbContext>(dbContextOptions).UseSqlite<AfonsoftDbContext>(connectionString).Options : new DbContextOptionsBuilder<AfonsoftDbContext>().UseSqlite<AfonsoftDbContext>(connectionString).Options;
                        break;
                    case EnumProvider.PostgreSQL:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<AfonsoftDbContext>(dbContextOptions).UseNpgsql<AfonsoftDbContext>(connectionString).Options : new DbContextOptionsBuilder<AfonsoftDbContext>().UseNpgsql<AfonsoftDbContext>(connectionString).Options;
                        break;
                    case EnumProvider.InMemory:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<AfonsoftDbContext>(dbContextOptions).UseInMemoryDatabase<AfonsoftDbContext>(connectionString).Options : new DbContextOptionsBuilder<AfonsoftDbContext>().UseInMemoryDatabase<AfonsoftDbContext>(connectionString).Options;
                        break;
                    default:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<AfonsoftDbContext>(dbContextOptions).UseInMemoryDatabase<AfonsoftDbContext>(connectionString).Options : new DbContextOptionsBuilder<AfonsoftDbContext>().UseInMemoryDatabase<AfonsoftDbContext>(connectionString).Options;
                        break;
                }

                return _dbContextOptions;
            }
        }

        /// <summary>
        /// Database.EnsureCreated with try/catch
        /// </summary>
        public virtual bool EnsureCreated()
        {
            try
            {
                return Database.EnsureCreated(); 
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Contrutor
        /// </summary>
        public AfonsoftDbContext(Action<AfonsoftEFOptions> configure) : base(Build(configure)) { EnsureCreated(); }
   
        private static DbContextOptions<AfonsoftDbContext> Build(Action<AfonsoftEFOptions> configure = null)
        {
            if (configure == null)
                return null;

            var opt = new AfonsoftEFOptions();
            configure(opt);
            return GetOptions(opt.Provider, opt.ConnectionString, opt.DbOptions);
        }

    }
}

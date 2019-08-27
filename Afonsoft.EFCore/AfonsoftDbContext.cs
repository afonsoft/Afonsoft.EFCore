using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Reflection;

namespace Afonsoft.EFCore
{
    /// <summary>
    /// RepositoryDbContext is a base of DbContext
    /// </summary>
    public  abstract class AfonsoftDbContext : DbContext
    {

        private static DbContextOptions<DbContext> GetOptions(EnumProvider provider, string connectionString = null, DbContextOptions<DbContext> dbContextOptions = null)
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

                DbContextOptions<DbContext> _dbContextOptions;
                switch (provider)
                {
                    case EnumProvider.MySQL:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<DbContext>(dbContextOptions).UseMySql(connectionString).Options : new DbContextOptionsBuilder<DbContext>().UseMySql(connectionString).Options;
                        break;
                    case EnumProvider.SQLServer:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<DbContext>(dbContextOptions).UseSqlServer(connectionString).Options : new DbContextOptionsBuilder<DbContext>().UseSqlServer(connectionString).Options;

                        break;
                    case EnumProvider.SQLite:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<DbContext>(dbContextOptions).UseSqlite(connectionString).Options : new DbContextOptionsBuilder<DbContext>().UseSqlite(connectionString).Options;
                        break;
                    case EnumProvider.PostgreSQL:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<DbContext>(dbContextOptions).UseNpgsql(connectionString).Options : new DbContextOptionsBuilder<DbContext>().UseNpgsql(connectionString).Options;
                        break;
                    case EnumProvider.InMemory:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<DbContext>(dbContextOptions).UseInMemoryDatabase(connectionString).Options : new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(connectionString).Options;
                        break;
                    default:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<DbContext>(dbContextOptions).UseInMemoryDatabase(connectionString).Options : new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(connectionString).Options;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {

            }
        }

        public AfonsoftDbContext(DbContextOptions<DbContext> options) : base(options)
        {
            var opt = new AfonsoftEFOptions();
            opt.Options = options;
            EnsureCreated();
        }

        /// <summary>
        /// Contrutor
        /// </summary>
        public AfonsoftDbContext(Action<AfonsoftEFOptions> configure) : base(Build(configure)) { EnsureCreated(); }

        private static DbContextOptions<DbContext> Build(Action<AfonsoftEFOptions> configure = null)
        {
            if (configure == null)
                return null;

            var opt = new AfonsoftEFOptions();
            configure(opt);
            opt.Options = GetOptions(opt.Provider, opt.ConnectionString, opt.Options);
            return opt.Options;
        }
        internal static AfonsoftEFOptions BuildOptions(Action<AfonsoftEFOptions> configure = null)
        {
            if (configure == null)
                return null;

            var opt = new AfonsoftEFOptions();
            configure(opt);
            opt.Options = GetOptions(opt.Provider, opt.ConnectionString, opt.Options);
            return opt;
        }
    }
}

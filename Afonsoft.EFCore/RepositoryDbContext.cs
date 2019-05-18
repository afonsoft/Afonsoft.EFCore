using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Reflection;

namespace Afonsoft.EFCore
{
    /// <summary>
    /// RepositoryDbContext is a base of DbContext
    /// </summary>
    public class RepositoryDbContext : DbContext
    {
        /// <summary>
        /// Provider in Use
        /// </summary>
        public EnumProvider Provider { get; }
        /// <summary>
        /// ConnectionString in Use
        /// </summary>
        public string ConnectionString { get; }

        private static DbContextOptions<RepositoryDbContext> GetOptions(EnumProvider provider, string connectionString = null, DbContextOptions<RepositoryDbContext> dbContextOptions = null)
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

                DbContextOptions<RepositoryDbContext> _dbContextOptions;
                switch (provider)
                {
                    case EnumProvider.MySQL:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<RepositoryDbContext>(dbContextOptions).UseMySql<RepositoryDbContext>(connectionString).Options : new DbContextOptionsBuilder<RepositoryDbContext>().UseMySql<RepositoryDbContext>(connectionString).Options;
                        break;
                    case EnumProvider.SQLServer:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<RepositoryDbContext>(dbContextOptions).UseSqlServer<RepositoryDbContext>(connectionString).Options : new DbContextOptionsBuilder<RepositoryDbContext>().UseSqlServer<RepositoryDbContext>(connectionString).Options;

                        break;
                    case EnumProvider.SQLite:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<RepositoryDbContext>(dbContextOptions).UseSqlite<RepositoryDbContext>(connectionString).Options : new DbContextOptionsBuilder<RepositoryDbContext>().UseSqlite<RepositoryDbContext>(connectionString).Options;
                        break;
                    case EnumProvider.PostgreSQL:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<RepositoryDbContext>(dbContextOptions).UseNpgsql<RepositoryDbContext>(connectionString).Options : new DbContextOptionsBuilder<RepositoryDbContext>().UseNpgsql<RepositoryDbContext>(connectionString).Options;
                        break;
                    case EnumProvider.InMemory:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<RepositoryDbContext>(dbContextOptions).UseInMemoryDatabase<RepositoryDbContext>(connectionString).Options : new DbContextOptionsBuilder<RepositoryDbContext>().UseInMemoryDatabase<RepositoryDbContext>(connectionString).Options;
                        break;
                    default:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<RepositoryDbContext>(dbContextOptions).UseInMemoryDatabase<RepositoryDbContext>(connectionString).Options : new DbContextOptionsBuilder<RepositoryDbContext>().UseInMemoryDatabase<RepositoryDbContext>(connectionString).Options;
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
        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options, EnumProvider provider, string connectionString) : base(GetOptions(provider, connectionString, options)) { Provider = provider; ConnectionString = connectionString; EnsureCreated(); }
        /// <summary>
        /// Contrutor
        /// </summary>
        public RepositoryDbContext(EnumProvider provider, string connectionString = null) : base(GetOptions(provider, connectionString)) { Provider = provider; ConnectionString = connectionString; EnsureCreated(); }
        /// <summary>
        /// Contrutor
        /// </summary>
        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options) : base(GetOptions(EnumProvider.InMemory, "", options)) { Provider = EnumProvider.InMemory; ConnectionString = ""; }
    }
}

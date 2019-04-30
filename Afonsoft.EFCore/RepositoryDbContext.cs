using Microsoft.EntityFrameworkCore;
using System;

namespace Afonsoft.EFCore
{
    /// <summary>
    /// RepositoryDbContext is a base of DbContext
    /// </summary>
    public class RepositoryDbContext : DbContext
    {
        public EnumSqlProvider Provider { get; private set; }
        public string ConnectionString { get; private set; }

        private static DbContextOptions<RepositoryDbContext> GetOptions(EnumSqlProvider provider, string connectionString = null, DbContextOptions<RepositoryDbContext> dbContextOptions = null)
        {
            if (string.IsNullOrEmpty(connectionString) && dbContextOptions != null)
                return dbContextOptions;
            else
            {
                if (string.IsNullOrEmpty(connectionString) && provider == EnumSqlProvider.SQLite)
                    connectionString = "Data Source=SQLite.db";

                if (string.IsNullOrEmpty(connectionString) && provider == EnumSqlProvider.InMemory)
                    connectionString = "InMemoryDataBase";

                if (string.IsNullOrEmpty(connectionString))
                    throw new ArgumentNullException(nameof(connectionString), "Não existe uma conexão.");

                DbContextOptions<RepositoryDbContext> _dbContextOptions;
                switch (provider)
                {
                    case EnumSqlProvider.MySQL:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<RepositoryDbContext>(dbContextOptions).UseMySql<RepositoryDbContext>(connectionString).Options : new DbContextOptionsBuilder<RepositoryDbContext>().UseMySql<RepositoryDbContext>(connectionString).Options;
                        break;
                    case EnumSqlProvider.SQLServer:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<RepositoryDbContext>(dbContextOptions).UseSqlServer<RepositoryDbContext>(connectionString).Options : new DbContextOptionsBuilder<RepositoryDbContext>().UseSqlServer<RepositoryDbContext>(connectionString).Options;

                        break;
                    case EnumSqlProvider.SQLite:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<RepositoryDbContext>(dbContextOptions).UseSqlite<RepositoryDbContext>(connectionString).Options : new DbContextOptionsBuilder<RepositoryDbContext>().UseSqlite<RepositoryDbContext>(connectionString).Options;
                        break;
                    case EnumSqlProvider.PostgreSQL:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<RepositoryDbContext>(dbContextOptions).UseNpgsql<RepositoryDbContext>(connectionString).Options : new DbContextOptionsBuilder<RepositoryDbContext>().UseNpgsql<RepositoryDbContext>(connectionString).Options;
                        break;
                    case EnumSqlProvider.InMemory:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<RepositoryDbContext>(dbContextOptions).UseInMemoryDatabase<RepositoryDbContext>(connectionString).Options : new DbContextOptionsBuilder<RepositoryDbContext>().UseInMemoryDatabase<RepositoryDbContext>(connectionString).Options;
                        break;
                    default:
                        _dbContextOptions = dbContextOptions != null ? new DbContextOptionsBuilder<RepositoryDbContext>(dbContextOptions).UseSqlite<RepositoryDbContext>(connectionString).Options : new DbContextOptionsBuilder<RepositoryDbContext>().UseSqlite<RepositoryDbContext>(connectionString).Options;
                        break;
                }
                return _dbContextOptions;
            }
        }

        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options, EnumSqlProvider provider, string connectionString) : base(GetOptions(provider, connectionString, options)) { Provider = provider; ConnectionString = connectionString; }
        public RepositoryDbContext(EnumSqlProvider provider, string connectionString = null) : base(GetOptions(provider, connectionString)) { Provider = provider; ConnectionString = connectionString; }
        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options) : base(GetOptions(EnumSqlProvider.SQLite, "Data Source=SQLite.db", options)) { Provider = EnumSqlProvider.SQLite; ConnectionString = "Data Source=SQLite.db"; }
    }
}

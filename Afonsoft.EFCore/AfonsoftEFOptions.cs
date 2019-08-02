using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Afonsoft.EFCore
{
    public class AfonsoftEFOptions
    {
        public DbContextOptions<RepositoryDbContext> DbOptions { get; set; }
        public EnumProvider Provider { get; set; } = EnumProvider.InMemory;
        public string ConnectionString { get; set; }
    }
}

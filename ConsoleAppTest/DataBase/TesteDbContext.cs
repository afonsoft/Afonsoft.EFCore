using Afonsoft.EFCore;
using ConsoleAppTest.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppTest.DataBase
{
    public class TesteDbContext : RepositoryDbContext
    {
        public TesteDbContext(DbContextOptions<RepositoryDbContext> options, EnumProvider provider, string connectionString) : base(options, provider, connectionString) { }
        public TesteDbContext(EnumProvider provider, string connectionString) : base(provider, connectionString) { }

        public TesteDbContext(EnumProvider provider) : base(provider) { }

        public virtual DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserModel>().HasKey(m => m.Id);
            base.OnModelCreating(builder);
        }
    }
}

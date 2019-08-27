using Afonsoft.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationTest
{
    public class TestDbContext : AfonsoftDbContext
    {
        public TestDbContext(DbContextOptions<DbContext> options) : base(options) { }
        public TestDbContext(Action<AfonsoftEFOptions> configure) : base(configure) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new TestUsersMap());
        }

        public DbSet<TestUser> Users { get; set; }
    }


    public class TestUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestUsersMap : IEntityTypeConfiguration<TestUser>
    {
        public void Configure(EntityTypeBuilder<TestUser> builder)
        {
            builder.HasKey(c => c.Id);

            builder.ToTable("Users");
            builder.Property(c => c.Id).HasColumnName("Id");
            builder.Property(c => c.Name).HasColumnName("Name");
        }
    }
}

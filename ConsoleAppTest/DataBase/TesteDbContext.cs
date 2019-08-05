using Afonsoft.EFCore;
using ConsoleAppTest.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppTest.DataBase
{
    public class AppDbContext : AfonsoftDbContext
    {
        public AppDbContext(Action<AfonsoftEFOptions> configure) : base(configure)
        {
        }

        public virtual DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserModel>().HasKey(m => m.Id);
            base.OnModelCreating(builder);
        }
    }
}

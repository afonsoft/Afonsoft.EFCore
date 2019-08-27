using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Afonsoft.EFCore
{
    public class AfonsoftEFOptions : IOptions<AfonsoftEFOptions> 
    {
        /// <summary>
        /// DbContextOptions
        /// </summary>
        public DbContextOptions<DbContext> Options { get; set; }

        /// <summary>
        /// Provider
        /// </summary>
        public EnumProvider Provider { get; set; } = EnumProvider.InMemory;
        /// <summary>
        /// ConnectionString
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public AfonsoftEFOptions Value => this;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TemplateProductName.Persistence
{
    public class TemplateProductNameDbContext : DbContext
    {
        private readonly string mappingAssembly;

        public TemplateProductNameDbContext(DbContextOptions options, string mappingAssembly) : base(options) => this.mappingAssembly = mappingAssembly;

        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load(mappingAssembly));
    }
}

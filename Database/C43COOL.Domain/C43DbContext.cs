using C43COOL.Domain.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Domain
{
    public class C43DbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Modules> Modules { get; set; }
        public DbSet<JobSchedule> JobSchedule { get; set; }
        public DbSet<Department> Department { get; set; }

        public DbSet<Relevance> Relevances { get; set; }

        public C43DbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

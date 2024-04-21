using Microsoft.EntityFrameworkCore;
using SuperTorus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperTorus.Infrastructure.Data
{
    public class Context: DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TorusDB>().HasIndex(x => x.Volume).IsDescending();
        }

        public DbSet<TorusDB> Torus { get; set; }
    }
}

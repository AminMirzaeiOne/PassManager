using Microsoft.EntityFrameworkCore;
using PassManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassManager.Infrastructure.Data
{
    public class AppDBContext : DbContext
    {
        public DbSet<PasswordItem> PasswordItems { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash).HasColumnType("BLOB");
            modelBuilder.Entity<User>()
                .Property(u => u.PasswordSalt).HasColumnType("BLOB");
            modelBuilder.Entity<User>()
                .Property(u => u.KeySalt).HasColumnType("BLOB");
        }
    }
}

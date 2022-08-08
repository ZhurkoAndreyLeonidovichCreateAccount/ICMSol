
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ApplicationDbContext: DbContext
    {
      
        public DbSet<User> Users { get; set; }
        public DbSet<PayerAccountNumber> PayerAccountNumbers  { get; set; }
        public DbSet<CheckPayerAccountNumber> CheckPayerAccountNumbers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
          

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CheckPayerAccountNumber>().HasKey(u => new { u.Name, u.UserEmail });
        }

    }
}

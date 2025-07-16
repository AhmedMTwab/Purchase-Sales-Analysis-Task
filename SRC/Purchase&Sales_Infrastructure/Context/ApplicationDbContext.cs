using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Purchase_Sales_Domain.Models;

namespace Purchase_Sales_Infrastructure.Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Product> products { get; set; }
        public DbSet<Sale> sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>().HasOne(s => s.Product).WithMany(p => p.sales).HasForeignKey(s => s.productId);
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Product>().Property(p=>p.id).ValueGeneratedNever();
        }
    }
}

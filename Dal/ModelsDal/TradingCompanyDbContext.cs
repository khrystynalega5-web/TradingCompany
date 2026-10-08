using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace TradingCompany.DAL.Models
{
    public class TradingCompanyDbContext : DbContext
    {
        public TradingCompanyDbContext()
        {
        }

        public TradingCompanyDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Category>  Categories { get; set; } = null!;
        public virtual DbSet <Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<Product>  Products { get; set; } = null!;
        public virtual DbSet <Sale> Sales { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                object value = GetValue(optionsBuilder);
            }

            static object GetValue(DbContextOptionsBuilder optionsBuilder)
            {
                return optionsBuilder.UseSqlServer(@"Server=(local);Database=TradingCompanyDB;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;");
            }
        }
    }
}
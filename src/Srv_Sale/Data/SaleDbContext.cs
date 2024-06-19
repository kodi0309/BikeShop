using Srv_Sale.Models;
using Microsoft.EntityFrameworkCore;

namespace Srv_Sale.Data
{
    public class SaleDbContext : DbContext
    {
        public SaleDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Sale> Sales { get; set; }
    }
}

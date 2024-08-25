using DB.TemporalTable.Api.Entities;
using DB.TemporalTable.Api.Profiles;
using Microsoft.EntityFrameworkCore;

namespace DB.TemporalTable.Api.Context
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Order { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
        }
    }
}

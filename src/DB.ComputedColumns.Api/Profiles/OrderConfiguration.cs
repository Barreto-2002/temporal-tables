using DB.TemporalTable.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DB.TemporalTable.Api.Profiles
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(nameof(Order), t => t.IsTemporal(p =>
            {
                p.HasPeriodStart("PeriodStart");
                p.HasPeriodEnd("PeriodEnd");
            }));

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Product)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(p => p.Quantity);

            builder.Property(p => p.UnitPrice)
                .HasColumnType("numeric(29, 2)");

            builder.Property(p => p.Description)
                .HasColumnType("varchar(50)")
                .IsRequired(false);

            builder.Property(p => p.CreatedBy)
                 .HasColumnType("varchar(50)");
        }
    }
}

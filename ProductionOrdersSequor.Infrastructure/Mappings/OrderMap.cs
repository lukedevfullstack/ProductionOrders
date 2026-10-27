using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionOrdersSequor.Domain.Entities;

namespace ProductionOrdersSequor.Infrastructure.Mappings;
public class OrderMap : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");
        builder.HasKey(o => o.OrderCode);
        builder.Property(o => o.OrderCode).HasColumnName("Order").HasColumnType("VARCHAR(50)");
        builder.Property(o => o.Quantity).HasColumnType("DECIMAL(18,2)");
        builder.Property(o => o.ProductCode).HasColumnType("VARCHAR(50)");

        builder.HasOne(o => o.Product)
              .WithMany(p => p.Orders)
              .HasForeignKey(o => o.ProductCode);
    }
}

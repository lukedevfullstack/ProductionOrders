using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionOrdersSequor.Domain.Entities;

namespace ProductionOrdersSequor.Infrastructure.Mappings;

public class ProductMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        builder.HasKey(p => p.ProductCode);
        builder.Property(p => p.ProductCode).HasColumnType("VARCHAR(50)");
        builder.Property(p => p.ProductDescription).HasColumnType("VARCHAR(50)");
        builder.Property(p => p.Image).HasColumnType("VARCHAR(500)");
        builder.Property(p => p.CycleTime).HasColumnType("DECIMAL(18,2)");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionOrdersSequor.Domain.Entities;

namespace ProductionOrdersSequor.Infrastructure.Mappings;

public class ProductionMap : IEntityTypeConfiguration<Production>
{
    public void Configure(EntityTypeBuilder<Production> builder)
    {
        builder.ToTable("Production");
        builder.HasKey(p => p.ID);
        builder.Property(p => p.ID).ValueGeneratedOnAdd();
        builder.Property(p => p.Email).HasColumnType("VARCHAR(100)");
        builder.Property(p => p.Order).HasColumnType("VARCHAR(50)");
        builder.Property(p => p.Date).HasColumnName("Date").HasColumnType("DATETIME");
        builder.Property(p => p.Quantity).HasColumnType("DECIMAL(18,2)");
        builder.Property(p => p.MaterialCode).HasColumnType("VARCHAR(50)");
        builder.Property(p => p.CycleTime).HasColumnType("DECIMAL(18,2)");
    }
}

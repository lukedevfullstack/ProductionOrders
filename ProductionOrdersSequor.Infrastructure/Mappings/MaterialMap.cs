using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionOrdersSequor.Domain.Entities;

namespace ProductionOrdersSequor.Infrastructure.Mappings;

public class MaterialMap : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Material");
        builder.HasKey(m => m.MaterialCode);
        builder.Property(m => m.MaterialCode).HasColumnType("VARCHAR(50)");
        builder.Property(m => m.MaterialDescription).HasColumnType("VARCHAR(500)");
    }
}

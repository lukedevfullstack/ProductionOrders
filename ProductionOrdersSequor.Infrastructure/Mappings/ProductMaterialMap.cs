using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionOrdersSequor.Domain.Entities;

namespace ProductionOrdersSequor.Infrastructure.Mappings;

public class ProductMaterialMap : IEntityTypeConfiguration<ProductMaterial>
{
    public void Configure(EntityTypeBuilder<ProductMaterial> builder)
    {
        builder.ToTable("ProductMaterial");
        builder.HasKey(pm => new { pm.ProductCode, pm.MaterialCode });

        builder.HasOne(pm => pm.Product)
              .WithMany(p => p.ProductMaterials)
              .HasForeignKey(pm => pm.ProductCode);

        builder.HasOne(pm => pm.Material)
              .WithMany(m => m.ProductMaterials)
              .HasForeignKey(pm => pm.MaterialCode);
    }
}
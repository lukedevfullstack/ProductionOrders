using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductionOrdersSequor.Domain.Entities;

namespace ProductionOrdersSequor.Infrastructure.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(u => u.Email);
        builder.Property(u => u.Email).HasColumnType("VARCHAR(100)");
        builder.Property(u => u.Name).HasColumnType("VARCHAR(50)");
        builder.Property(u => u.InitialDate).HasColumnType("DATETIME");
        builder.Property(u => u.EndDate).HasColumnType("DATETIME");
    }
}
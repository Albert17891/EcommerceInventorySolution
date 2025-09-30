using EcommerceInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInventory.Infrastructure.Configuration;
public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(p => p.Price)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(p => p.Stock)
               .IsRequired()
               .IsConcurrencyToken();

        builder.Property(p => p.Id)
               .HasColumnType("uuid")
               .ValueGeneratedOnAdd();
    }
}
using EcommerceInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceInventory.Infrastructure.Configuration;
public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {      
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
               .ValueGeneratedNever(); 

        builder.Property(m => m.EventType)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(m => m.Payload)
               .IsRequired()
               .HasColumnType("nvarchar(max)");
       

        builder.Property(m => m.CreatedAt)
               .IsRequired();

        builder.Property(m => m.ProcessedAt);

        builder.Property(m => m.IsProcessed)
               .HasDefaultValue(false);

        builder.Property(m => m.RetryCount)
               .HasDefaultValue(0);
    }
}

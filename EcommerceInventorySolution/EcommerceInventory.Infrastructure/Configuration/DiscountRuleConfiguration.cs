using EcommerceInventory.Domain.Entities.Discounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceInventory.Infrastructure.Configuration;
public class DiscountRuleConfiguration : IEntityTypeConfiguration<DiscountRule>
{
    public void Configure(EntityTypeBuilder<DiscountRule> builder)
    {      

        builder.HasKey(d => d.Id);        

        builder.Property(d => d.CardType)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(d => d.Type)
               .IsRequired();

        builder.Property(d => d.DiscountPercentage)
               .HasColumnType("decimal(18,2)")
               .IsRequired(false); 

        builder.Property(d => d.FixedAmount)
               .HasColumnType("decimal(18,2)")
               .IsRequired(false); 

        builder.Property(d => d.MinimumPurchaseAmount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(d => d.ValidFrom)
               .IsRequired();

        builder.Property(d => d.ValidTo)
               .IsRequired();

        builder.Property(d => d.Active)
               .IsRequired();        

        builder.HasIndex(d => d.CardType).IsUnique();      
    }
}

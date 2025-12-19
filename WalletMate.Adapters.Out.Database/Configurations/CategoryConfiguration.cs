using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletMate.Adapters.Out.Database.Entities;

namespace WalletMate.Adapters.Out.Database.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        builder.HasMany(t => t.TransactionCategories)
            .WithOne(tc => tc.Category)
            .HasForeignKey(tc => tc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
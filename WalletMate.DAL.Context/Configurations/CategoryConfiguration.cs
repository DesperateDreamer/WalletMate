using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletMate.DAL.Entities;

namespace WalletMate.DAL.Context.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
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
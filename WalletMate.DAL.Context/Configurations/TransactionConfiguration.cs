using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletMate.DAL.Entities;

namespace WalletMate.DAL.Context.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        
        builder.HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(t => t.TransactionCategories)
            .WithOne(tc => tc.Transaction)
            .HasForeignKey(tc => tc.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
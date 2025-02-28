using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Domain.Movie;

namespace Movie.Infrastructure.Persistence.Common.Configurations;

public class PurchaseConfigurations : BaseConfiguration, IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.ToTable(nameof(Purchase)).HasKey(p => p.Id);

        builder.HasOne(p => p.User)
            .WithMany()
            .IsRequired()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(p => p.Movie)
            .WithMany()
            .IsRequired()
            .HasForeignKey(p => p.MovieId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
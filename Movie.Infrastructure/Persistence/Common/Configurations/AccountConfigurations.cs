using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Domain.Account;

namespace Movie.Infrastructure.Persistence.Common.Configurations;

public class AccountConfigurations : BaseConfiguration, IEntityTypeConfiguration<Domain.Account.User>
{
    public void Configure(EntityTypeBuilder<Domain.Account.User> builder)
    {
        builder.ToTable(nameof(User)).HasKey(c => c.Id);
        builder.Property(x => x.Password).HasMaxLength(StringMaxLength);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(StringMaxLength);
    }
}
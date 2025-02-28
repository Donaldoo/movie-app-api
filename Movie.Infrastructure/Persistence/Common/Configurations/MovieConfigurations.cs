using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Movie.Infrastructure.Persistence.Common.Configurations;

public class MovieConfigurations : BaseConfiguration, IEntityTypeConfiguration<Domain.Movie.Movie>
{
    public void Configure(EntityTypeBuilder<Domain.Movie.Movie> builder)
    {
        builder.ToTable(nameof(Domain.Movie.Movie)).HasKey(c => c.Id);
    }
}
using Microsoft.EntityFrameworkCore;
using Movie.Application.Common.Data;

namespace Movie.Infrastructure.Persistence.Common.Data;

public class EfDataMigrator : IDataMigrator
{
    private readonly AppDbContext _appDbContext;

    public EfDataMigrator(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Migrate()
    {
        if (_appDbContext.Database.GetPendingMigrations().Any())
            _appDbContext.Database.Migrate();
    }
}
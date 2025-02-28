using MediatR;
using Microsoft.EntityFrameworkCore;
using Movie.Application.Common;
using Movie.Application.Movie;
using Movie.Domain.Movie;

namespace Movie.Infrastructure.Persistence.Movie;

internal sealed class GetPurchasedMoviesHandler : IRequestHandler<GetPurchasedMovies, IList<MovieResponse>>
{
    private readonly AppDbContext _db;
    private readonly ICurrentUser _currentUser;

    public GetPurchasedMoviesHandler(AppDbContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<IList<MovieResponse>> Handle(GetPurchasedMovies request, CancellationToken cancellationToken)
    {
        return await _db.Movies
            .Where(m => _db.Purchases.Any(p => p.MovieId == m.Id && p.OrderStatus == OrderStatus.Completed && p.UserId == _currentUser.UserId))
            .Select(x => new MovieResponse
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                VideoUrl = x.VideoUrl,
                ThumbnailUrl = x.CoverImageUrl
            })
            .ToListAsync(cancellationToken);
    }
}
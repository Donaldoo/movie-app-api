using MediatR;
using Microsoft.EntityFrameworkCore;
using Movie.Application.Common;
using Movie.Application.Movie;
using Movie.Domain.Movie;

namespace Movie.Infrastructure.Persistence.Movie;

internal sealed class GetMoviesQueryHandler : IRequestHandler<GetMoviesQuery, IList<MovieResponse>>
{
    private readonly AppDbContext _db;
    private readonly ICurrentUser _currentUser;

    public GetMoviesQueryHandler(AppDbContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<IList<MovieResponse>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        return await _db.Movies.Select(m => new MovieResponse
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            ThumbnailUrl = m.CoverImageUrl,
            VideoUrl = m.VideoUrl,
            Price = m.Price,
            IsPurchased = _currentUser != null && _db.Purchases.Any(p =>
                p.MovieId == m.Id && p.OrderStatus == OrderStatus.Completed && p.UserId == _currentUser.UserId)
        }).ToListAsync(cancellationToken);
    }
}
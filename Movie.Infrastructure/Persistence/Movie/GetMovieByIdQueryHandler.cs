using MediatR;
using Microsoft.EntityFrameworkCore;
using Movie.Application.Movie;

namespace Movie.Infrastructure.Persistence.Movie;

internal sealed class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieVideoResponse>
{
    private readonly AppDbContext _db;

    public GetMovieByIdQueryHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<MovieVideoResponse> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        return await _db
            .Movies
            .AsNoTracking()
            .Where(m => m.Id == request.Id).Select(m => new MovieVideoResponse
            {
                Title = m.Title,
                VideoUrl = m.VideoUrl,
                Price = m.Price,
                Description = m.Description,
                ThumbnailUrl = m.CoverImageUrl
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movie.Application.Common;
using Movie.Application.Movie;
using Movie.Domain.Movie;

namespace Movie.Infrastructure.Persistence.Movie;

internal sealed class GetMovieVideoQueryHandler : IRequestHandler<GetMovieVideoQuery, MovieVideoResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly ICurrentUser _currentUser;

    public GetMovieVideoQueryHandler(AppDbContext dbContext, ICurrentUser currentUser)
    {
        _dbContext = dbContext;
        _currentUser = currentUser;
    }

    public async Task<MovieVideoResponse> Handle(GetMovieVideoQuery request, CancellationToken cancellationToken)
    {
        return await  _dbContext
            .Movies
            .AsNoTracking()
            .Where(m => m.Id == request.MovieId &&
                        _dbContext.Purchases.Any(p => p.MovieId == m.Id && p.OrderStatus == OrderStatus.Completed && p.UserId == _currentUser.UserId))
            .Select(m => new MovieVideoResponse
            {
                Title = m.Title,
                VideoUrl = m.VideoUrl,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
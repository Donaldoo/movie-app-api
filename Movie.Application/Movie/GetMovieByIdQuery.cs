using MediatR;

namespace Movie.Application.Movie;

public record GetMovieByIdQuery(Guid Id) : IRequest<MovieVideoResponse>;

public record MovieVideoResponse
{
    public string Title { get; init; }
    public string VideoUrl { get; init; }
    public decimal Price { get; init; }
    public string Description { get; init; }
    public string ThumbnailUrl { get; init; }
}
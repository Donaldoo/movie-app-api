using MediatR;

namespace Movie.Application.Movie;

public record GetMoviesQuery : IRequest<IList<MovieResponse>>;

public record MovieResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string ThumbnailUrl { get; init; }
    public string VideoUrl { get; init; }
    public decimal Price { get; init; }
    public bool IsPurchased { get; init; }
    public decimal RentPrice { get; set; }
}
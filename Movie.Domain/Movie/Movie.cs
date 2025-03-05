using Movie.Domain.Common;

namespace Movie.Domain.Movie;

public record Movie : EntityWithGuid
{
    public string Title { get; init; }
    public string Description { get; init; }
    public string CoverImageUrl { get; init; }
    public string VideoUrl { get; init; }
    public decimal Price { get; init; }
    public decimal RentPrice { get; set; }
}
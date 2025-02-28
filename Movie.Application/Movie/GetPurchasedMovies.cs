using MediatR;

namespace Movie.Application.Movie;

public record GetPurchasedMovies : IRequest<IList<MovieResponse>>;
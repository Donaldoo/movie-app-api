using MediatR;

namespace Movie.Application.Movie;

public record GetMovieVideoQuery(Guid MovieId) : IRequest<MovieVideoResponse>;
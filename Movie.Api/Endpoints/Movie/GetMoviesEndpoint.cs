using MediatR;
using Movie.Application.Movie;

namespace Movie.Api.Endpoints.Movie;

public static class GetMoviesEndpoint
{
    public static IEndpointRouteBuilder MapGetMovies(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/movies",
            async (IMediator mediator, CancellationToken cancellationToken) =>
                await mediator.Send(new GetMoviesQuery(), cancellationToken));
        return app;
    }
}
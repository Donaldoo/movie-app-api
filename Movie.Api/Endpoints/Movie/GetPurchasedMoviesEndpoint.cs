using MediatR;
using Movie.Application.Movie;

namespace Movie.Api.Endpoints.Movie;

public static class GetPurchasedMoviesEndpoint
{
    public static IEndpointRouteBuilder MapGetPurchasedMovies(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/movies-tv",
            async (IMediator mediator, CancellationToken cancellationToken) =>
                await mediator.Send(new GetPurchasedMovies(), cancellationToken));
        return app;
    }
}
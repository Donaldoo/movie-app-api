using MediatR;
using Movie.Application.Movie;

namespace Movie.Api.Endpoints.Movie;

public static class GetMovieVideoEndpoint
{
    public static IEndpointRouteBuilder MapGetMovieVideo(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/movie-video",
            async ([AsParameters] GetMovieVideoQuery request, IMediator mediator) => await mediator.Send(request));
        return app;
    }
}
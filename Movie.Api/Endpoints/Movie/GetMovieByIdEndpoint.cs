using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movie.Application.Movie;

namespace Movie.Api.Endpoints.Movie;

public static class GetMovieByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetMovieById(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/movie", async ([AsParameters] GetMovieByIdQuery request, IMediator mediator) 
            => await mediator.Send(request));
        return app;
    }
}
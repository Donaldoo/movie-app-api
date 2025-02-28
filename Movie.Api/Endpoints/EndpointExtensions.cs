using Movie.Api.Endpoints.Account;
using Movie.Api.Endpoints.Auth;
using Movie.Api.Endpoints.Movie;

namespace Movie.Api.Endpoints;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        //call methods for endpoint
        app.MapAuth();
        app.MapAccountEndpoints();
        app.MapMovieEndpoints();
        return app;
    }
}
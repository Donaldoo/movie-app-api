namespace Movie.Api.Endpoints.Movie;

public static class MovieEndpointExtensions
{
    public static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetMovies();
        app.MapGetMovieById();
        app.MapWebhook();
        app.MapSavePurchase();
        app.MapGetMovieVideo();
        app.MapGetPurchasedMovies();
        return app;
    }
}
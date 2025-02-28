namespace Movie.Api.Endpoints.Account;

public static class AccountEndpointExtensions 
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCreateAccount();
        return app;
    }
}
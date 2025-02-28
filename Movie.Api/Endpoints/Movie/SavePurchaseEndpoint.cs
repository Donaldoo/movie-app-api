using MediatR;
using Movie.Application.Movie.SavePurchase;

namespace Movie.Api.Endpoints.Movie;

public static class SavePurchaseEndpoint
{
    public static IEndpointRouteBuilder MapSavePurchase(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/save-order", async (SavePurchaseCommand command, IMediator mediator) =>
            await mediator.Send(command));
        return app;
    }
}
using MediatR;
using Movie.Application.Movie.ProcessPayment;
using Newtonsoft.Json.Linq;

namespace Movie.Api.Endpoints.Movie;

public static class WebhookEndpoint
{
    public static IEndpointRouteBuilder MapWebhook(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/webhooks/pokpay", async (HttpRequest request, ILogger<Program> logger, IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var requestBody = await new StreamReader(request.Body).ReadToEndAsync(cancellationToken);
                var payload = JObject.Parse(requestBody);

                var paymentId = payload.SelectToken("orderId")?.ToObject<Guid>();
                if (paymentId == null)
                {
                    logger.LogWarning("Payment ID not found in webhook payload");
                    return Results.BadRequest("Payment ID is required.");
                }
                
                await mediator.Send(new ProcessPayment { PaymentId = paymentId.Value },
                    cancellationToken);

                logger.LogInformation($"Processed webhook for Payment ID: {paymentId}");
                return Results.Ok();
            }
            catch (KeyNotFoundException ex)
            {
                logger.LogWarning(ex.Message);
                return Results.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error handling webhook: {ex.Message}");
                return Results.BadRequest();
            }
        });
        return app;
    }
}
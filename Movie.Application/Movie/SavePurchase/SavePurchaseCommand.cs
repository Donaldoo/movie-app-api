using MediatR;
using Movie.Domain.Movie;

namespace Movie.Application.Movie.SavePurchase;

public record SavePurchaseCommand : IRequest<SaveOrderResponse>
{
    public Guid? Id { get; init; }
    public Guid? PaymentId { get; init; }
    public Guid ProductId { get; init; }
    public OrderStatus Status { get; init; }
    public Uri RedirectUrl { get; init; }
    public Uri FailRedirectUrl { get; init; }
    public int ExpiresAfterMinutes { get; set; }
}

public record SaveOrderResponse
{
    public Guid OrderId { get; init; }
    public string PokOrderUrl { get; init; }
}
using MediatR;

namespace Movie.Application.Movie.ProcessPayment;

public record ProcessPayment : IRequest<Guid>
{
    public Guid PaymentId { get; init; }
}
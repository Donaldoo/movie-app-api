namespace Movie.Application.PaymentClient;

public interface IPaymentClient
{
    Task AuthorizePok(CancellationToken cancellationToken);
    Task<PokConfirmation> PayWithPok(PayWithPokCommand request, CancellationToken cancellationToken);
    Task<PokOrderConfirmation> GetPokConfirmationByOrderId(Guid orderId, CancellationToken cancellationToken);
}
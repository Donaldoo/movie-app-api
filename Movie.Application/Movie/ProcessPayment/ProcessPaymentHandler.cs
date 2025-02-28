using MediatR;
using Movie.Application.Common.Data;
using Movie.Application.Common.Exceptions;
using Movie.Application.PaymentClient;
using Movie.Domain.Movie;

namespace Movie.Application.Movie.ProcessPayment;

internal sealed class ProcessPaymentHandler : IRequestHandler<ProcessPayment, Guid>
{
    private readonly IPaymentClient _paymentClient;
    private readonly IDataWriter _dataWriter;
    private readonly IGenericQuery _genericQuery;

    public ProcessPaymentHandler(IPaymentClient paymentClient, IDataWriter dataWriter, IGenericQuery genericQuery)
    {
        _paymentClient = paymentClient;
        _dataWriter = dataWriter;
        _genericQuery = genericQuery;
    }

    public async Task<Guid> Handle(ProcessPayment request, CancellationToken cancellationToken)
    {
        var orderConfirmation = await _paymentClient.GetPokConfirmationByOrderId(request.PaymentId, cancellationToken);

        if (!orderConfirmation.IsCompleted)
            return Guid.Empty;

        var existingOrder =
            await _genericQuery.GetOneEntityByPropertyAsync<Purchase>("PaymentId", request.PaymentId);

        if (existingOrder == null)
            throw new EntityNotFoundException($"No existing order found for Payment ID: {request.PaymentId}");

        await _dataWriter.UpdateOneAsync<Purchase>(p => p with
        {
            OrderStatus = OrderStatus.Completed
        }, existingOrder.Id);
        await _dataWriter.SaveAsync(cancellationToken);
        return existingOrder.Id;
    }
}
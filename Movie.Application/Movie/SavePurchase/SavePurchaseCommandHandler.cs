using MediatR;
using Movie.Application.Common;
using Movie.Application.Common.Data;
using Movie.Application.PaymentClient;
using Movie.Domain.Movie;

namespace Movie.Application.Movie.SavePurchase;

internal sealed class SavePurchaseCommandHandler : IRequestHandler<SavePurchaseCommand, SaveOrderResponse>
{
    private readonly IPaymentClient _paymentClient;
    private readonly ICurrentUser _currentUser;
    private readonly IGenericQuery _genericQuery;
    private readonly IDataWriter _dataWriter;

    public SavePurchaseCommandHandler(IPaymentClient paymentClient, ICurrentUser currentUser, IGenericQuery genericQuery, IDataWriter dataWriter)
    {
        _paymentClient = paymentClient;
        _currentUser = currentUser;
        _genericQuery = genericQuery;
        _dataWriter = dataWriter;
    }

    public async Task<SaveOrderResponse> Handle(SavePurchaseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var productFromDb = await _genericQuery.GetByIdAsync<Domain.Movie.Movie>(request.ProductId);
            var products = new[]
            {
                new Product
                {
                    Name = productFromDb.Title,
                    Quantity = 1,
                    Price =request.PurchaseType == PurchaseType.Buy ? productFromDb.Price : productFromDb.RentPrice,
                }
            };
            await _paymentClient.AuthorizePok(cancellationToken);
            var pokConfirmation = await _paymentClient.PayWithPok(new PayWithPokCommand
            {
                AutoCapture = true,
                CurrencyCode = "EUR",
                ExpiresAfterMinutes = request.ExpiresAfterMinutes,
                FailRedirectUrl = request.FailRedirectUrl,
                Products = products,
                RedirectUrl = request.RedirectUrl,
                WebhookUrl = new Uri("https://700d-141-98-141-126.ngrok-free.app/api/webhooks/pokpay")
            }, cancellationToken);

            var order = new Purchase
            {
                MovieId = request.ProductId,
                OrderStatus = request.Status,
                UserId = _currentUser.UserId,
                PaymentId = pokConfirmation.OrderId,
            };
            _dataWriter.Add(order);
            await _dataWriter.SaveAsync(cancellationToken);

            return new SaveOrderResponse
            {
                OrderId = order.Id,
                PokOrderUrl = pokConfirmation.ConfirmationUrl
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
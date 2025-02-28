using System.Net.Http.Headers;
using Movie.Application.Common;
using Movie.Application.Common.Extensions;
using Newtonsoft.Json.Linq;

namespace Movie.Application.PaymentClient;

public class PaymentClient : IPaymentClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _client;
    private string _authToken;
    private readonly ISettings _settings;

    public PaymentClient(IHttpClientFactory httpClientFactory, ISettings settings)
    {
        _httpClientFactory = httpClientFactory;
        _settings = settings;
        _client = _httpClientFactory.CreateClient();
        _authToken = "";
    }

    public async Task AuthorizePok(CancellationToken cancellationToken)
    {
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        var requestUrl = _settings.PokBaseUrl + "/auth/sdk/login";
        var credentials = new
        {
            keyId = "834e2921-9043-4c50-8e07-ff51c05aa066",
            keySecret = "mOIO2FPgBT6U7YTr2bFCltv+RV63T6f1iloGD67J"
        };
        try
        {
            var response = await _client.PostAsync(requestUrl, credentials.AsJson(), cancellationToken);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync(cancellationToken);
            var token = JObject.Parse(res).SelectToken("data.accessToken")?.ToObject<string>();
            _authToken = token;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PokConfirmation> PayWithPok(PayWithPokCommand request, CancellationToken cancellationToken)
    {
        request.ExpiresAfterMinutes = 10;
        var requestUrl = _settings.PokBaseUrl + "/merchants/bd778276-5f15-457e-bcee-bacc881b2e63/sdk-orders";
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _authToken);
        var response = await _client.PostAsync(requestUrl, request.AsJson(), cancellationToken);
        var result = await response.Content.ReadAsStringAsync(cancellationToken);
        var orderId = JObject.Parse(result).SelectToken("data.sdkOrder.id")?.ToObject<string>();
        var url = JObject.Parse(result).SelectToken("data.sdkOrder._self.confirmUrl")?.ToObject<string>();
        return new PokConfirmation
        {
            OrderId = new Guid(orderId),
            ConfirmationUrl = url
        };
    }

    public async Task<PokOrderConfirmation> GetPokConfirmationByOrderId(Guid orderId, CancellationToken cancellationToken)
    {
        try
        {
            var requestUrl = _settings.PokBaseUrl + "/sdk-orders/" + orderId;
            var response = await _httpClientFactory.CreateClient().GetAsync(requestUrl, cancellationToken);
            var result = await response.Content.ReadAsStringAsync(cancellationToken);
            var isCompleted = JObject.Parse(result).SelectToken("data.sdkOrder.isCompleted")!.ToObject<bool>();
            var expiresAt = JObject.Parse(result).SelectToken("data.sdkOrder.expiresAt")!.ToObject<DateTime>();
            return new PokOrderConfirmation
            {
                IsCompleted = isCompleted,
                ExpiresAt = expiresAt
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return new PokOrderConfirmation()
        {
            IsCompleted = false
        };
    }
}
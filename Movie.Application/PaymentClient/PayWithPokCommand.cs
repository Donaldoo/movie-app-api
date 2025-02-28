namespace Movie.Application.PaymentClient;

public class PayWithPokCommand
{
    public IList<Product> Products { get; init; }
    public string CurrencyCode { get; init; }
    public bool AutoCapture { get; init; } = true;
    public Uri RedirectUrl { get; init; }
    public Uri FailRedirectUrl { get; init; }
    public int ExpiresAfterMinutes { get; set; }
    public string WebhookUrl { get; set; }
}

public class PokConfirmation
{
    public string ConfirmationUrl { get; init; }
    public Guid OrderId { get; init; }
}

public class Product
{
    public string Name { get; init; }
    public long Quantity { get; init; }
    public decimal Price { get; set; }
}

public record PokOrderConfirmation
{
    public bool IsCompleted { get; init; }
    public DateTime ExpiresAt { get; init; }
}
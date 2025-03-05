using Movie.Domain.Account;
using Movie.Domain.Common;

namespace Movie.Domain.Movie;

public record Purchase : EntityWithGuid
{
    public Guid UserId { get; init; }
    public Guid MovieId { get; init; }
    public Guid? PaymentId { get; init; }
    public OrderStatus OrderStatus { get; set; }
    public DateTimeOffset PurchaseDate { get; init; } = DateTimeOffset.UtcNow;
    public PurchaseType PurchaseType { get; set; }
    public User User { get; init; }
    public Movie Movie { get; init; }
}

public enum PurchaseType
{
    Buy,
    Rent
}
public enum OrderStatus
{
    Pending,
    Completed,
    Cancelled
}

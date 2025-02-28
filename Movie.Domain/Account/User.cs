using Movie.Domain.Common;

namespace Movie.Domain.Account;

public record User : EntityWithGuid
{
    public string Email { get; init; }
    public string Password { get; init; }
}
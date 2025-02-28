using MediatR;

namespace Movie.Application.Account.Authenticator;

public record GetAuthenticationInfoQuery(Guid UserId) : IRequest<AuthenticationInfoResponse>;

public record AuthenticationInfoResponse
{
    public Guid UserId { get; init; }
    public string Email { get; init; }
}
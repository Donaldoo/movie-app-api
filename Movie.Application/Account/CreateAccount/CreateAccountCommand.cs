using MediatR;
using Movie.Application.Account.Authenticator;

namespace Movie.Application.Account.CreateAccount;

public record CreateAccountCommand : IRequest<AuthenticationInfoResponse>
{
    public string Email { get; init; }
    public string Password { get; init; }
}
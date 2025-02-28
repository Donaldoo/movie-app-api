using MediatR;
using Movie.Domain.Account;

namespace Movie.Application.Account.Authenticator;

public record GetUserByEmailQuery : IRequest<User>
{
    public required string Email { get; init; }
}
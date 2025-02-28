using MediatR;
using Movie.Application.Account.Authenticator;
using Movie.Application.Common.Data;
using Movie.Domain.Account;

namespace Movie.Application.Account.CreateAccount;

internal class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AuthenticationInfoResponse>
{
    private readonly IDataWriter _dataWriter;
    private readonly IMediator _mediator;
    private readonly IPasswordHasher _passwordHasher;

    public CreateAccountCommandHandler(IPasswordHasher passwordHasher, IDataWriter dataWriter, IMediator mediator)
    {
        _passwordHasher = passwordHasher;
        _dataWriter = dataWriter;
        _mediator = mediator;
    }

    public async Task<AuthenticationInfoResponse> Handle(CreateAccountCommand request,
        CancellationToken cancellationToken)
    {
        var account = new User
        {
            Email = request.Email,
            Password = _passwordHasher.HashPassword(request.Password)
        };
        await _dataWriter.Add(account).SaveAsync(cancellationToken);

        return await _mediator.Send(new GetAuthenticationInfoQuery(account.Id), cancellationToken);
    }
}
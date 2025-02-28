using MediatR;
using Movie.Application.Common.Exceptions;
using Movie.Application.Common.Requests;

namespace Movie.Application.Common.Behaviours;

public class AuthenticateBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ICurrentUser _currentUser;

    public AuthenticateBehaviour(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!(request is IAuthenticatedRequest))
            return await next();

        if (_currentUser == null)
            throw new AuthorizationException();

        return await next();
    }
}
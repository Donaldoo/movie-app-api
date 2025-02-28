using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movie.Api.Auth;
using Movie.Application.Account.CreateAccount;

namespace Movie.Api.Endpoints.Account;

public static class CreateAccountEndpoint
{ 
    public static IEndpointRouteBuilder MapCreateAccount(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/register", async ([FromBody] CreateAccountCommand request, IMediator mediator,  ITokenGenerator tokenGenerator) =>
        {
           var account = await mediator.Send(request);
           return await tokenGenerator.GenerateAsync(account);
        });
        
        return app;
    }
}
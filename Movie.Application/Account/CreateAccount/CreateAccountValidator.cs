using FluentValidation;
using MediatR;
using Movie.Application.Account.Authenticator;
using Movie.Application.Internationalization;

namespace Movie.Application.Account.CreateAccount;

public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
{
    private readonly IMediator _mediator;

    public CreateAccountValidator(IMediator mediator, ILanguageResource resource)
    {
        _mediator = mediator;
        RuleFor(f => f.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(resource.FieldRequired(nameof(CreateAccountCommand.Email)))
            .MustAsync(BeUnique)
            .WithMessage(resource.UserEmailAlreadyExists());

        RuleFor(f => f.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(resource.FieldRequired(nameof(CreateAccountCommand.Password)))
            .MinimumLength(6).WithMessage(resource.PasswordTooShort());
    }

    private async Task<bool> BeUnique(string email, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetUserByEmailQuery
        {
            Email = email
        }, cancellationToken);

        return user == null;
    }
}
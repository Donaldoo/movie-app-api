using Movie.Application.Account.Authenticator;

namespace Movie.Api.Auth;

public interface ITokenGenerator
{
    Task<TokenDto> GenerateAsync(AuthenticationInfoResponse user);
}

public class TokenDto
{
    public string Token { get; set; }
}
using System.Security.Claims;
using System.Security.Principal;
using Movie.Application.Account.Authenticator;
using Newtonsoft.Json;

namespace Movie.Api.Common.Extensions;

public static class PrincipalExtensions
{
    public static AuthenticationInfoResponse GetUserAuthenticationInfo(this IPrincipal currentPrincipal)
    {
        var identity = currentPrincipal.Identity as ClaimsIdentity;
        var claim = identity?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);
        return claim != null ? JsonConvert.DeserializeObject<AuthenticationInfoResponse>(claim.Value) : null;
    }
}
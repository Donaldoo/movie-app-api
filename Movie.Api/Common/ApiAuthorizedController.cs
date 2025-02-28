using Microsoft.AspNetCore.Authorization;

namespace Movie.Api.Common;

[Authorize]
public class ApiAuthorizedController : ApiControllerBase
{
        
}
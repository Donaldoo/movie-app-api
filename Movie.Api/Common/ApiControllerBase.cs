using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movie.Api.Common.Models;
using Movie.Application.Common;
using X.PagedList;

namespace Movie.Api.Common;

[Route("[controller]")]
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    private ICurrentUser _currentUser;
    protected ICurrentUser CurrentUser => _currentUser ??= HttpContext.RequestServices.GetService<ICurrentUser>();

    protected PagedListDto<T> PagedListResult<T>(IPagedList<T> pagedList)
    {
        return new PagedListDto<T>(pagedList);
    }
}
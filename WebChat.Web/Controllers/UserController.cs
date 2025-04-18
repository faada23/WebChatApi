using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
 
[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class UserController : ControllerBase {
    public IUserService UserService{get; set;}
    public UserController(IUserService userService){
        UserService = userService;
    }

    [HttpGet("Users")]
    public async Task<ActionResult<PagedResponse<GetUserResponse>>> Users(
        [FromQuery] string? filter, 
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var pagParams = (page.HasValue && pageSize.HasValue)
        ? new PaginationParameters { Page = page.Value, PageSize = pageSize.Value }
        : null;
        
        var pageUsers = await UserService.GetUsers(filter, pagParams);

        return Ok(pageUsers);
    }

}
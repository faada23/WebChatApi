using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
[ApiController]
[Route("[controller]")]
[Authorize]
public class ChatController : ControllerBase {

    public IChatService ChatService {get;set;}
    public ChatController(IChatService chatService){
        ChatService = chatService;
    }

    [HttpPost("PrivateChat")]
    public async Task<IActionResult> CreatePrivateChat([FromQuery] int joinUserId)
    {   
        int currentUserId = GetCurrentUserId();
        var result = await ChatService.CreatePrivateChat(currentUserId,joinUserId);

        if(!result) return StatusCode(500,"Error while creating chat"); 
        return Ok();
    }

    [HttpGet("PrivateChat")]
    public async Task<ActionResult<PagedResponse<GetChatResponse>>> GetPrivateChats(
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var pagParams = (page.HasValue && pageSize.HasValue)
        ? new PaginationParameters { Page = page.Value, PageSize = pageSize.Value }
        : null;

        int currentUserId = GetCurrentUserId();
        var chats = await ChatService.GetPrivateChats(currentUserId,pagParams);
        return Ok(chats);
    }


    private int GetCurrentUserId()
    {
        var userId = User.FindFirstValue("Id");

        return int.Parse(userId);
    }

}
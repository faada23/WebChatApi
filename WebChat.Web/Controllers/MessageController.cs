using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
[ApiController]
[Route("[controller]")]
[Authorize]
public class MessageController : ControllerBase {

    public IMessageService MessageService {get;set;}
    public MessageController(IMessageService messageService){
        MessageService = messageService;
    }

    [HttpGet("Messages/{chatId}")]
    public async Task<ActionResult<PagedResponse<GetMessagesResponse>>> GetMessagesByChatId(
        int chatId,
        [FromQuery] int? page,
        [FromQuery] int? pageSize)
    {
        var pagParams = (page.HasValue && pageSize.HasValue)
        ? new PaginationParameters { Page = page.Value, PageSize = pageSize.Value }
        : null;

        int currentUserId = GetCurrentUserId();
        var messages = await MessageService.GetMessages(chatId,pagParams);
        return Ok(messages);
    }

    private int GetCurrentUserId()
    {
        var userId = User.FindFirstValue("Id");

        return int.Parse(userId);
    }

}
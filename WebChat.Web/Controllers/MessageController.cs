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
    public async Task<IActionResult> GetMessagesByChatId(int chatId,[FromQuery] PaginationParameters? pagParams)
    {   
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
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

    [HttpGet("MessagesByChatId")]
    public async Task<IActionResult> GetMessagesByChatId(int chatId)
    {   
        int currentUserId = GetCurrentUserId();
        var messages = await MessageService.GetMessages(chatId);
        return Ok(messages);
    }

    private int GetCurrentUserId()
    {
        var userId = User.FindFirstValue("Id");

        return int.Parse(userId);
    }

}
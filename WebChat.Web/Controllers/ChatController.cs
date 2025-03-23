using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
[ApiController]
[Route("[controller]")]
[Authorize]
public class ChatController : ControllerBase {

    public ChatController(){

    }

    [HttpGet("UserChats")]
    public async Task<ActionResult<string>> GetUserChats(){

        return Ok(new {result = "working!"});
    }


}
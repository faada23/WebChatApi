using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
[ApiController]
[Route("[controller]")]
[Authorize]
public class ChatController : ControllerBase {

    public ChatController(){

    }

    [HttpGet("UserChats")]
    public async Task<ActionResult<string[]>> GetUserChats(){
        string[] messages = new string[]{
            "1231231",
            "312412412",
            "ff34f4f",
            "lol"
        };

        return Ok(messages);
    }


}
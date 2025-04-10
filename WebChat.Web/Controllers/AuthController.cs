using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
 
[ApiController]
[Route("[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase {
    public IAuthService AuthService{get; set;}
    public AuthController(IAuthService authService){
        AuthService = authService;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<string?>> Login([FromForm] IFormCollection form){

        var username = form["username"].ToString();
        var password = form["password"].ToString();

        string? token = await AuthService.Login(username, password);
        if(token != null)
        {
            Response.Cookies.Append("JwtCookie",token); 
            return Ok(new {token});
        }

        return StatusCode(500,"Wrong authentication data");
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromForm] IFormCollection form){

        var username = form["username"].ToString();
        var password = form["password"].ToString();

        var result = await AuthService.Register(username, password);
        
        if(!result) return StatusCode(500,"Error while creating new account");
        return Ok();
    }


    [HttpGet("Check")]
    public async Task<IActionResult> IsUserAuthenticated(){


        if(User.Identity != null && User.Identity.IsAuthenticated)
        {
            return Ok();
        }
        return Unauthorized();
    }
    
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout(){
        Response.Cookies.Delete("JwtCookie");
        return Ok();
    }
}
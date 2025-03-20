using Microsoft.AspNetCore.Mvc;

public class AuthController : ControllerBase {
    public IAuthService AuthService{get; set;}
    public AuthController(IAuthService authService){
        AuthService = authService;
    }

    public async Task<IActionResult> Login(LoginRequest request){
        string? token = await AuthService.Login(request);
        if(token != null)
        {
            Response.Cookies.Append("JwtCookie",token); 
            return Ok();
        }

        return StatusCode(500,"Wrong authentication data");
    }
}
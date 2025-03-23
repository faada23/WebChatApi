using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService {
    public IRepository<User> UserRepository{get; set;}
    public IJwtProvider JwtProvider{get; set;}

    public AuthService(IRepository<User> userRepository, IJwtProvider jwtProvider){
        UserRepository = userRepository;
        JwtProvider = jwtProvider;
    }

    public async Task<string?> Login(string reqUsername, string reqHashPassword){
        var user = await UserRepository.GetByFilter(u => u.Username == reqUsername);

        if(user != null)
        {
            var passVerifResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, reqHashPassword);

            if(passVerifResult == PasswordVerificationResult.Success)
            {
                var token = JwtProvider.GenerateToken(user);
                return token;
            }
        }

        return null;
    }

    public async Task Register(string reqUsername, string reqHashPassword)
    {
        User user = new User{
            Username = reqUsername,
            PasswordHash = reqHashPassword
        };

        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, user.PasswordHash);

        await UserRepository.Insert(user);
    }
}
public class AuthService : IAuthService {
    public IRepository<User> UserRepository{get; set;}

    public AuthService(IRepository<User> userRepository){
        UserRepository = userRepository;
    }

    public Task<string?> Login(LoginRequest request){
        throw new NotImplementedException();
    }

    public Task Register(RegisterRequest request)
    {
        throw new NotImplementedException();
    }
}
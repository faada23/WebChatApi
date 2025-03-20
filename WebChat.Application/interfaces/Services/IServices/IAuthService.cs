public interface IAuthService {
    public Task<string?> Login(LoginRequest request);

    public Task Register(RegisterRequest request);
}
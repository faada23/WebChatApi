public interface IAuthService {
    public Task<string?> Login(string reqUsername, string reqHashPassword);

    public Task<bool> Register(string reqUsername, string reqHashPassword);
}
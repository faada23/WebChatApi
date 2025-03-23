public interface IAuthService {
    public Task<string?> Login(string reqUsername, string reqHashPassword);

    public Task Register(string reqUsername, string reqHashPassword);
}
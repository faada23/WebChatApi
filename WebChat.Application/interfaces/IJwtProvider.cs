public interface IJwtProvider {
    public string? GenerateToken(User user);
}
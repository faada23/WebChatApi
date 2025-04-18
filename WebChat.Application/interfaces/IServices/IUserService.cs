public interface IUserService{
    Task<PagedResponse<GetUserResponse>> GetUsers(string? filter, PaginationParameters pagParams);
}
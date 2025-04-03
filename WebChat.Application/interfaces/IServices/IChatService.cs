public interface IChatService {
    public Task CreatePrivateChat(int createUserId, int joinUserId);
    public Task<PagedResponse<GetChatResponse>> GetPrivateChats(int userId,PaginationParameters pagParams);
}
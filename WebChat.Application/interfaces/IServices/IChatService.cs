public interface IChatService {
    public Task CreatePrivateChat(int createUserId, int joinUserId);
    public Task<List<GetChatResponse>> GetPrivateChats(int userId);
}
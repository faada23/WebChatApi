public interface IMessageService
{
    public Task<List<GetMessagesResponse>> GetMessages(int chatId);
}
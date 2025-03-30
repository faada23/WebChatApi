public interface IMessageService
{
    public Task<List<GetMessagesResponse>> GetMessages(int chatId);
    public Task<Message> CreateMessage(CreateMessage messagesRequest);
}
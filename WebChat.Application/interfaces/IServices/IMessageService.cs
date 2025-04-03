public interface IMessageService
{
    public Task<PagedResponse<GetMessagesResponse>> GetMessages(int chatId, PaginationParameters pagParams);
    public Task<Message> CreateMessage(CreateMessage messagesRequest);
}
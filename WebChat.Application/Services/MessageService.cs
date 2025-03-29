


public class MessageService: IMessageService
{       
    public IRepository<Message> MessageRepository {get;set;}

    public MessageService(IRepository<Message> messageRepository){
        MessageRepository = messageRepository;
    }

    public async Task<List<GetMessagesResponse>> GetMessages(int chatId)
    {
        var messages = await MessageRepository.GetAll(
            filter: x => x.ChatId == chatId,
            orderBy: x => x.OrderByDescending(x => x.CreatedDate),
            includeProperties:"Sender"       
            );

        return messages.Select(message => new GetMessagesResponse
            (   
                message.Id,
                message.Content,
                message.CreatedDate,
                new GetUserResponse(
                    message.SenderId,
                    message.Sender!.Username
                ),
                message.ChatId
        )).ToList();
    }
}
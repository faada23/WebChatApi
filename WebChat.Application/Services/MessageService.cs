public class MessageService: IMessageService
{       
    public IRepository<Message> MessageRepository {get;set;}

    public MessageService(IRepository<Message> messageRepository){
        MessageRepository = messageRepository;
    }

    public async Task<PagedResponse<GetMessagesResponse>> GetMessages(int chatId,PaginationParameters pagParams)
    {
        var messages = await MessageRepository.GetAll(
            filter: x => x.ChatId == chatId,
            pagParams: pagParams,
            orderBy: x => x.OrderByDescending(x => x.CreatedDate),
            includeProperties:"Sender"       
            );

        var messagesDto = messages.Select(message => new GetMessagesResponse
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

        //returning list in rigth order
        messagesDto.Reverse();

        return new PagedResponse<GetMessagesResponse>(
            messagesDto,
            messages.CurrentPage,
            messages.PageSize,
            messages.TotalItems,
            messages.TotalPages
        );
    }

    public async Task<Message> CreateMessage(CreateMessage messagesRequest){
        var message = new Message {
            Content = messagesRequest.Content,
            SenderId = messagesRequest.SenderId,
            ChatId = messagesRequest.ChatId
        };

        await MessageRepository.Insert(message);

        return message;
    }
}
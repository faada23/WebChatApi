

public class ChatService : IChatService
{       
    public IRepository<Chat> ChatRepository {get;set;}
    public IRepository<User> UserRepository {get;set;}

    public ChatService(IRepository<Chat> chatRepository, IRepository<User> userRepository){
        ChatRepository = chatRepository;
        UserRepository = userRepository;
    }


    public async Task<bool> CreatePrivateChat(int createUserId, int joinUserId)
    {   
        var existingChat = await ChatRepository.GetByFilter(
            filter: c => c.ChatType == ChatType.Private &&
                        c.Users!.Any(u => u.Id == createUserId) &&
                        c.Users!.Any(u => u.Id == joinUserId),
            includeProperties: "Users"
        );

        var users = await UserRepository.GetAll(
            filter: x => x.Id == createUserId || x.Id == joinUserId
        );

        if(users.Count != 2) return false;

        if(existingChat != null) return false;

        var chat = new Chat {
            Name = $"Chat between {users[0].Username} and {users[1].Username}",
            ChatType = ChatType.Private,
            Users = users
        };

        await ChatRepository.Insert(chat);
        return true;
    }

    public async Task<PagedResponse<GetChatResponse>> GetPrivateChats(int userId,PaginationParameters pagParams){
        var privateChats = await ChatRepository.GetAll(
            filter: x => x.ChatType == ChatType.Private &&
                x.Users!.Any(x => x.Id == userId),
            pagParams: pagParams,
            includeProperties:"Users"
            );
        
        var chatsDto = privateChats.Select(chat => new GetChatResponse
            (   
                chat.Id,
                chat.Users!.FirstOrDefault(u => u.Id != userId)!.Username,
                chat.ChatType,
                chat.Users!.Select(u => new GetUserResponse
                (
                    u.Id,
                    u.Username
                )).ToList()
        )).ToList();

        return new PagedResponse<GetChatResponse>(
            chatsDto,
            privateChats.CurrentPage,
            privateChats.PageSize,
            privateChats.TotalItems,
            privateChats.TotalPages
        );
    }


}
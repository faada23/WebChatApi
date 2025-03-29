

public class ChatService : IChatService
{       
    public IRepository<Chat> ChatRepository {get;set;}
    public IRepository<User> UserRepository {get;set;}

    public ChatService(IRepository<Chat> chatRepository, IRepository<User> userRepository){
        ChatRepository = chatRepository;
        UserRepository = userRepository;
    }


    public async Task CreatePrivateChat(int createUserId, int joinUserId)
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

        if(users.Count != 2) throw new InvalidDataException("Users search error");

        if(existingChat != null) throw new InvalidOperationException("This chat is already exists");

        var chat = new Chat {
            Name = $"Chat between {users[0].Username} and {users[1].Username}",
            ChatType = ChatType.Private,
            Users = users
        };

        await ChatRepository.Insert(chat);
    }

    public async Task<List<GetChatResponse>> GetPrivateChats(int userId){
        var privateChats = await ChatRepository.GetAll(
            filter: x => x.ChatType == ChatType.Private &&
                x.Users!.Any(x => x.Id == userId),
                includeProperties:"Users"
            );
        
        return privateChats.Select(chat => new GetChatResponse
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

    }


}
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[Authorize]
public class ChatHub : Hub {
    public IRepository<Message> MessageRepository {get;set;}
    public IMessageService MessageService {get;set;}

    public ChatHub(IRepository<Message> messageRepository, IMessageService messageService){
        MessageRepository = messageRepository;
        MessageService = messageService;
    }
    

    public async Task JoinChat(int chatId){
        var groupName = GetGroupName(chatId);
        await Groups.AddToGroupAsync(Context.ConnectionId,groupName);
        await Clients.Group(groupName).SendAsync("ReceiveMessage", "LOL");    
    }

    public async Task SendMessage(int chatId, string content){
        var groupName = GetGroupName(chatId);
        var userId = GetUserId();
        var userName = GetUsername();

        var createMessage = new CreateMessage(
            content,
            userId,
            chatId
        );

        var message = await MessageService.CreateMessage(createMessage); 

        var messageToSent = new GetMessage(
            message.Id,
            message.Content,
            message.CreatedDate,
            new GetUserResponse(
                userId,
                userName
            )
        );

        await Clients.Group(groupName).SendAsync("ReceiveMessage", messageToSent);
    }

    public string GetGroupName(int chatId){

        return $"chat_{chatId}";
    }

    private int GetUserId()
    {
        return int.Parse(
            GetUserClaims().FindFirstValue("Id") ?? throw new NullReferenceException(" userId null reference")
            );
    }

    private string GetUsername()
    {
        return GetUserClaims().FindFirstValue("Username") ?? throw new NullReferenceException(" username null reference");
    }

    private ClaimsPrincipal GetUserClaims()
    {
        return Context.User ?? throw new HubException("User not authenticated");
    }
}


public record GetChatResponse(
    int Id,
    string Name,
    ChatType ChatType,
    List<GetUserResponse> Users
);
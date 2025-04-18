public record GetMessagesResponse(
    int Id,
    string? content,
    DateTime CreatedDate,
    GetUserResponse Sender,
    int ChatId
);
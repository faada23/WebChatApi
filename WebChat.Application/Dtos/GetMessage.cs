public record GetMessage(
    int Id,
    string? Content,
    DateTime CreatedDate,
    GetUserResponse Sender
);
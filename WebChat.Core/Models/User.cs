using System.ComponentModel.DataAnnotations.Schema;

public class User {
    public int Id{get; set;}
    public string Username{get; set;} = null!;
    public string PasswordHash{get;set;} = null!;

    public ICollection<Chat>? Chats{get;set;}
}
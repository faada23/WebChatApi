using System.ComponentModel.DataAnnotations.Schema;

public class Chat
{
    public int Id { get; set; }
    public string Name {get;set;}
    public ChatType ChatType {get;set;}
    
    public ICollection<User>? Users {get;set;} = new List<User>();
    public ICollection<Message>? Messages { get; set; } = new List<Message>();
}
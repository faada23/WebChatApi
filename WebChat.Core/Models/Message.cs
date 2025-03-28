public class Message {
    public int Id {get;set;}
    public string? Content {get;set;}
    public DateTime CreatedDate {get;set;} = DateTime.UtcNow;

    public int ChatId { get; set; }
    public Chat? Chat { get; set; }

}
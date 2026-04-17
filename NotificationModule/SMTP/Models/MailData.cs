namespace NotificationModule.SMTP.Models;

public class MailData {
    
    public List<string> To { get; set; }
    public List<string> Bcc { get; set; }

    public List<string> Cc { get;set; }

  
    public string? From { get;set; }

    public string? DisplayName { get;set; }

    public string? ReplyTo { get; set;}

    public string? ReplyToName { get; set;}


    public string Subject { get;set; }

    public string? Body { get;set; }

    
}

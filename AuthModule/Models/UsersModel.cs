using System.ComponentModel.DataAnnotations;

namespace AuthModule.Models;

public class UsersModel
{
    [Required , Key]
    public Guid Id { get; set; } =  Guid.NewGuid();
    
    [Required ]
    [EmailAddress]
    public string email { get; set; }
    
    [Required ]
    public string password_Hashed { get; set; }
    
    public bool IsLocked { get; set; } = false;
    
    public DateTimeOffset? LockoutEnd { get; set; }
    
    public int AccessFailedCount { get; set; } = 0;
    
    [Required ]
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    
    public string? DisplayName { get; set; }
}
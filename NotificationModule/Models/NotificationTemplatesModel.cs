using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NotificationModule.Models;

public class NotificationTemplatesModel
{
    [Key]
    [Required]
    public string id { get; set; }

    [Required]
    [MaxLength(200)]
    public string name { get; set; }

    [Required]
    [MaxLength(50)]
    public string type { get; set; }

    [Required]
    [MaxLength(50)]
    public string channel { get; set; }

    public string? SubjectTemplate { get; set; }

    [Required]
    public string contentTemplate { get; set; }

    public Dictionary<string, object>? Variables { get; set; }

    [Required]
    public bool isActive { get; set; } = true;

    [Required]
    [MaxLength(10)]
    public string language { get; set; } = "en";

    [Required]
    public DateTime created_at { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime updated_at { get; set; } = DateTime.UtcNow;
}
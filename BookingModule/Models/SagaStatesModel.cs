using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace BookingModule.Models;
[Index(nameof(saga_id),IsUnique =true)]
public class SagaStatesModel
{
    [Required , Key]
    public Guid id { get; set; } = Guid.NewGuid();
    [Required]
    public Guid saga_id { get; set; } =Guid.NewGuid();
    [Required] 
    public string saga_type { get; set; } = "booking";
    [Required]
    public SagaTypes status { get; set; } = SagaTypes.Started;
    [MaxLength(100)]
    public string? current_step { get; set; }
    [Required]
    public DateTime started_at { get; set; } = DateTime.UtcNow;
    public DateTime completed_at { get; set; } 
    [Required]
    public DateTime last_updated_at { get; set; } =  DateTime.UtcNow;
    [Required]
    public int retry_count { get; set; } = 0;
    [Required]
    public int max_retries { get; set; } = 3;
    public string? error_message { get; set; }
    [MaxLength(50)]
    public string? error_code { get; set; }
    public Dictionary<string, object>? metadata { get; set; }


}
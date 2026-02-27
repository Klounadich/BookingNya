using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BookingModule.Models;
[Index(nameof(saga_id),IsUnique =true)]
public class SagaStepsModel
{
    [Required , Key]
    public Guid id { get; set; }
    [Required]
    public Guid saga_id { get; set; }
    [Required, MaxLength(100)]
    public string step_name { get; set; }
    [Required]
    public int step_order { get; set; }
    [Required ,MaxLength(50)]
    public string status {get;set;}
    public DateTime? started_at { get; set; }
    public DateTime ?completed_at { get; set; }
    public DateTime? compensated_at { get; set; }
    [Required]
    public bool is_compensated { get; set; }= false;
    public string? error_message { get; set; }
    public string? error_code { get; set; }
    public Dictionary<string, object>? request_payload { get; set; }
    public Dictionary<string, object>? responce_payload { get; set; }
    [Required]
    public DateTime created_at { get; set; } = DateTime.UtcNow;

}
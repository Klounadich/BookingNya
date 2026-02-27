using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace BookingModule.Models;
[Index(nameof(saga_id),IsUnique =true)]
public class ProcessedCommandsModel
{
    [Required , Key]
    public Guid id { get; set; } = Guid.NewGuid();
    [Required]
    public Guid saga_id { get; set; }
    [Required, MaxLength(100)]
    public  string command_type {get; set;}
    [MaxLength(100)]
    public string? command_id {get; set;}

    public CommandsStatus status { get; set; } = CommandsStatus.Processed;
    [Required]
    public DateTime processed_at {get; set;} = DateTime.Now;
    public Dictionary<string , object>? result_payload {get; set;}
    public string? error_message {get; set;}
    [Required]
    public DateTime created_at {get; set;} =  DateTime.UtcNow;
}
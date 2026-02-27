using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Enums;

namespace NotificationModule.Models;

public class NotificationModel
{
    [Key]
        
        [Required]
        public Guid id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid? saga_id { get; set; }

        [Required]
        public Guid? booking_id { get; set; }

        [Required]
        [MaxLength(100)]
        
        public string type { get; set; } // confirmation, failure, reminder

        [Required]
        [MaxLength(50)]
        
        public string channel { get; set; } // email, sms, push

        [Required]
        [MaxLength(255)]
        
        public string recipient { get; set; }

        [MaxLength(500)]
        
        public string? subject { get; set; }

        [Required]
        public string content { get; set; }

        [Required] [MaxLength(50)] 
        public NotificationStatus status { get; set; } = NotificationStatus.Pending;

       
        public int attempts { get; set; } = 0;

        
        public int max_attempts { get; set; } = 3;

       
        public DateTime? scheduled_at { get; set; }

        
        public DateTime? sent_at { get; set; }

        
        public DateTime? delivered_at { get; set; }

        
        public string? error_message { get; set; }

        [MaxLength(100)]
        
        public string? template_id { get; set; }

        [Required]
        [MaxLength(10)]
        
        public string language { get; set; } = "en";

        [Required]
        
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        [Required]
       
        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        
        public Dictionary<string, object>? metadata { get; set; }
}
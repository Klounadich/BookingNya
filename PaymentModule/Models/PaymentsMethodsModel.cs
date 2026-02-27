using System.ComponentModel.DataAnnotations;

namespace PaymentModule.Models;

public class PaymentsMethodsModel
{
    [Key]
    [Required]
    public Guid id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string customer_id { get; set; }

    [Required]
    [MaxLength(50)]
    public string payment_method_type { get; set; }

    [Required]
    [MaxLength(50)]
    public string provider { get; set; }

    [Required]
    [MaxLength(255)]
    public string provider_token { get; set; }

    [Required]
    public bool is_default { get; set; } = false;

    [Required]
    public bool is_verified { get; set; } = false;

    [MaxLength(4)]
    public string? card_last4 { get; set; }

    [MaxLength(20)]
    public string? card_brand { get; set; }

    public int? card_expiry_month { get; set; }

    public int? card_expiry_year { get; set; }

    public Dictionary<string, object>? billing_address { get; set; }

    [Required]
    public int failure_count { get; set; } = 0;

    public DateTime? disabled_at { get; set; }

    [Required]
    public DateTime created_at { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime updated_at { get; set; } = DateTime.UtcNow;

    public Dictionary<string, object>? metadata { get; set; }
}
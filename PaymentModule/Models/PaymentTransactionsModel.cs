using System.ComponentModel.DataAnnotations;

namespace PaymentModule.Models;

public class PaymentTransactionsModel
{
    [Key]
    [Required]
    public Guid id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid payment_id { get; set; }

    public Guid? saga_id { get; set; }

    [Required]
    [MaxLength(50)]
    public string type { get; set; }

    [Required]
    public decimal amount { get; set; }

    [Required]
    [MaxLength(3)]
    public string currency { get; set; } = "USD";

    [MaxLength(100)]
    public string? gateway_transaction_id { get; set; }

    public Dictionary<string, object>? gateway_response { get; set; }

    [Required]
    [MaxLength(50)]
    public string status { get; set; }

    public string? failure_reason { get; set; }

    public Guid? parent_transaction_id { get; set; }

    public DateTime? processed_at { get; set; }

    [Required]
    public DateTime created_at { get; set; } = DateTime.UtcNow;

    public Dictionary<string, object>? metadata { get; set; }
}
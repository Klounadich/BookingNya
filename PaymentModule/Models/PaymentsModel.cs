using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace PaymentModule.Models;

public class PaymentsModel
{
    [Key]
    [Required]
    public Guid id { get; set; } = Guid.NewGuid();

    public Guid? saga_id { get; set; }

    public Guid? booking_id { get; set; }

    [MaxLength(100)]
    public string? transaction_id { get; set; }

    [MaxLength(100)]
    public string? reservation_id { get; set; }

    [Required]
    public decimal amount { get; set; }

    [Required]
    [MaxLength(3)]
    public string currency { get; set; } = "USD";

    [Required]
    [MaxLength(50)]
    public string payment_method { get; set; }

    [MaxLength(50)]
    public string? payment_gateway { get; set; }

    [Required]
    [MaxLength(50)]
    public PaymentStatus status { get; set; } = PaymentStatus.Pending;

    [MaxLength(100)]
    public string? customer_id { get; set; }

    [MaxLength(255)]
    public string? customer_email { get; set; }

    [MaxLength(50)]
    public string? customer_phone { get; set; }

    public Dictionary<string, object>? gateway_response { get; set; }

    public string? failure_reason { get; set; }

    public DateTime? authorized_at { get; set; }

    public DateTime? captured_at { get; set; }

    public DateTime? refunded_at { get; set; }

    public DateTime? expires_at { get; set; }

    [Required]
    public DateTime created_at { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime updated_at { get; set; } = DateTime.UtcNow;

    public Dictionary<string, object>? metadata { get; set; }
}
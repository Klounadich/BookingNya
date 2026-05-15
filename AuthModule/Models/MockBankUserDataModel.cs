using System.ComponentModel.DataAnnotations;

namespace PaymentModule.Models;

public class MockBankUserDataModel
{
    [Key]
    [Required]
    public Guid user_id { get; set; }

    public decimal balance { get; set; } = 0;
}
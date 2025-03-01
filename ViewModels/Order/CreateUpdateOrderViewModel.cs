using System.ComponentModel.DataAnnotations;

namespace Pizzeria.ViewModels.Order;

public class CreateUpdateOrderViewModel
{
    public int Id { get; set; } = 0;
    [Required]
    public string? Fio { get; set; }
    [Required]
    [Phone]
    public string? Phone { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? City { get; set; }
    [Required]
    public string? Address { get; set; }
}
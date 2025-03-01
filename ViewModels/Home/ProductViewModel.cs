using Pizzeria.Models;

namespace Pizzeria.ViewModels.Home;

public class ProductViewModel
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public float Weight { get; set; }
    public float Calories { get; set; }
    public decimal Price { get; set; }
    public string? Brand { get; set; }
    public Category? Category { get; set; }
}
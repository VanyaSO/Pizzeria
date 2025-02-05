namespace Pizzeria.Models;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public DateTime DateOfPublication { get; set; }
}
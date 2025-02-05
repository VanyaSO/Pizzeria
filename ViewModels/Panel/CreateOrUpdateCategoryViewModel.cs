using System.ComponentModel.DataAnnotations;

namespace Pizzeria.ViewModels;

public class CreateOrUpdateCategoryViewModel
{
    public string? Id { get; set; }
    
    [Required(ErrorMessage = "Название категории обязательно.")]
    [StringLength(100, ErrorMessage = "Название категории не может превышать 50 символов.")]
    [Display(Name = "Название")]
    public string Name { get; set; }

    [StringLength(200, ErrorMessage = "Описание не может превышать 200 символов.")]
    [Display(Name = "Описание")]
    public string? Description { get; set; }

    [Display(Name = "Изображение")]
    public IFormFile? File { get; set; }

    public string? Image { get; set; }
    public string? ImageFullName { get; set; }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Pizzeria.ViewModels;

public class CreateOrUpdateUserViewModel
{
    public string? Id { get; set; }
    
    [Required(ErrorMessage = "Введите емейл")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Не корректный емейл")]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Введите год рождения")]
    [Display(Name = "Год рождения")]
    [Range(1900, 2024, ErrorMessage = "Год рождения должен быть в диапазоне от 1900 до 2024")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Введите номер телефона")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Номер телефона")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Номер телефона должен содержать 10 цифр")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    [StringLength(100, ErrorMessage = "Пароль должен содержать не менее 6 символов", MinimumLength = 6)]
    public string? Password { get; set; }
    
    [DataType(DataType.Password)]
    [Display(Name = "Новый пароль")]
    [StringLength(100, ErrorMessage = "Пароль должен содержать не менее 6 символов", MinimumLength = 6)]
    public string? NewPassword { get; set; }
}
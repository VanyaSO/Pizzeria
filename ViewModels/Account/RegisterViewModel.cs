using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Pizzeria.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Введите емейл")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Не корректный емейл")]
    [Display(Name = "Email")]
    [Remote(action: "IsEmailInUse", controller: "Account", ErrorMessage = "Емейл адрес уже используется")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Введите год рождения")]
    [Display(Name = "Год рождения")]
    [Range(1900, 2024, ErrorMessage = "Год рождения должен быть в диапазоне от 1900 до 2024")] // Пример валидации года
    public int Year { get; set; }

    [Required(ErrorMessage = "Введите номер телефона")]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Номер телефона")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Номер телефона должен содержать 10 цифр")] // Пример валидации номера
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    [StringLength(100, ErrorMessage = "Пароль должен содержать не менее 6 символов", MinimumLength = 6)] // Валидация длины пароля
    public string? Password { get; set; }

    [Required(ErrorMessage = "Подтвердите пароль")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    public string? PasswordConfirm { get; set; }
}
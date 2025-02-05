using System.ComponentModel.DataAnnotations;

namespace Pizzeria.ViewModels;

public class ResetPasswordViewModel
{
    [Required(ErrorMessage = "Введите новый пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Новый пароль")]
    [MinLength(5, ErrorMessage = "Минимальная длина пароля 5 символов")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Подтвердите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [MinLength(5, ErrorMessage = "Минимальная длина пароля 5 символов")]
    public string PasswordConfirm { get; set; }

    public string Email { get; set; }

    public string Token { get; set; }
}
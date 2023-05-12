using System.ComponentModel.DataAnnotations;

namespace BackAPI.ViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных регисрируемого пользователяв
    /// </summary>
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = null!;
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; } = null!;
    }
}

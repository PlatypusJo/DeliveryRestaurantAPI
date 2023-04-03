using System.ComponentModel.DataAnnotations;

namespace BackAPI.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = null!;

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
    }
}

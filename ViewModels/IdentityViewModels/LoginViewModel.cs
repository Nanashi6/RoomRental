using System.ComponentModel.DataAnnotations;

namespace RoomRental.ViewModels.IdentityViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Необходимо указать логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Необходимо указать пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
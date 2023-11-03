using System.ComponentModel.DataAnnotations;

namespace RoomRental.ViewModels.IdentityViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}

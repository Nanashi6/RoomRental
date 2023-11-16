using System.ComponentModel.DataAnnotations;

namespace RoomRental.ViewModels.IdentityViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}

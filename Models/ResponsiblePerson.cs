using System.ComponentModel.DataAnnotations;

namespace RoomRental.Models;

public partial class ResponsiblePerson
{
    [Display(Name = "Идентификатор")]
    public int PersonId { get; set; }

    [Required(ErrorMessage = "Не указана фамилия")]
    [Display(Name = "Фамилия")]
    public string Surname { get; set; }

    [Required(ErrorMessage = "Не указано имя")]
    [Display(Name = "Имя")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Не указано отчество")]
    [Display(Name = "Отчество")]
    public string Lastname { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}

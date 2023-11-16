using System.ComponentModel.DataAnnotations;

namespace RoomRental.Models;

public partial class ResponsiblePerson
{
    [Display(Name = "Идентификатор")]
    public int PersonId { get; set; }
    [Display(Name = "Фамилия")]
    public string Surname { get; set; }
    [Display(Name = "Имя")]
    public string Name { get; set; }
    [Display(Name = "Отчество")]
    public string Lastname { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}

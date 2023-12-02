using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RoomRental.Models;
public partial class Organization
{
    [Display(Name = "Идентификатор")]
    public int OrganizationId { get; set; }

    [Required(ErrorMessage = "Не указано название")]
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Не указан почтовый адрес")]
    [Display(Name = "Почтовый адрес")]
    public string PostalAddress { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();
    [JsonIgnore]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    [JsonIgnore]
    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}

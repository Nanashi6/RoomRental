using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomRental.Models;

public partial class Room
{
    [Display(Name = "Идентификатор")]
    public int RoomId { get; set; }
    [Display(Name = "Здание")]
    public int BuildingId { get; set; }
    [Display(Name = "Площадь")]
    public decimal Area { get; set; }
    [Display(Name = "Описание")]
    public string Description { get; set; }
    [NotMapped]
    public List<IFormFile> Photos { get; set; }
    [Display(Name = "Здание")]
    [ValidateNever]
    public virtual Building Building { get; set; }
    [Display(Name = "Фото")]
    public virtual ICollection<RoomImage> RoomImages { get; set; } = new List<RoomImage>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}

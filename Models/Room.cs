using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomRental.Models;

public partial class Room
{
    [Display(Name = "Идентификатор")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Не указано здание")]
    [Display(Name = "Здание")]
    public int BuildingId { get; set; }

	[Required(ErrorMessage = "Не указан номер помещения")]
	[Display(Name = "Номер помещения")]
	public int RoomNumber { get; set; }

	[Required(ErrorMessage = "Не указана площадь")]
    [Display(Name = "Площадь")]
    [Range(0, double.MaxValue, ErrorMessage = "Значение не может быть меньше нуля")]
    public decimal Area { get; set; }

	[Required(ErrorMessage = "Не указано описание")]
    [Display(Name = "Описание")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Не указано фото")]
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

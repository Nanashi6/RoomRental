using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomRental.Models;

public partial class Building
{
    [Display(Name = "Идентификатор")]
    public int BuildingId { get; set; }

    [Required(ErrorMessage = "Не указано название")]
    [Display(Name = "Название здания")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Не указана организация-владелей")]
    [Display(Name = "Организация-владелец")]
    public int OwnerOrganizationId { get; set; }

    [Required(ErrorMessage = "Не указан почтовый адрес")]
    [Display(Name = "Почтовый адрес")]
    public string PostalAddress { get; set; }

    [Required(ErrorMessage = "Не указана этажность")]
    [Display(Name = "Этажность")]
    public int Floors { get; set; }

    [Required(ErrorMessage = "Не указано описание")]
    [Display(Name = "Описание")]
    public string Description { get; set; }

    [Display(Name = "План этажа")]
    [BindNever]
    [ValidateNever]
    public string FloorPlan { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Не указано изображение")]
    public IFormFile FloorPlanImage { get; set; }

    [ValidateNever]
    [Display(Name = "Организация-владелец")]
    public virtual Organization OwnerOrganization { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomRental.Models;

public partial class Building
{
    [Display(Name = "Идентификатор")]
    public int BuildingId { get; set; }
    [Display(Name = "Название здания")]
    public string Name { get; set; }
    [Display(Name = "Организация-владелец")]
    public int OwnerOrganizationId { get; set; }
    [Display(Name = "Почтовый адрес")]
    public string PostalAddress { get; set; }
    [Display(Name = "Этажность")]
    public int Floors { get; set; }
    [Display(Name = "Описание")]
    public string Description { get; set; }
    [Display(Name = "План этажа")]
    [BindNever]
    [ValidateNever]
    public string FloorPlan { get; set; }
    [NotMapped]
    public IFormFile FloorPlanImage { get; set; }
    [ValidateNever]
    public virtual Organization OwnerOrganization { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}

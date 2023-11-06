using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RoomRental.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace RoomRental.Models;

public partial class Rental
{
    [Display(Name = "Идентификатор")]
    public int RentalId { get; set; }
    [Display(Name = "Комната")]
    public int RoomId { get; set; }
    [Display(Name = "Организация-арендатор")]
    public int RentalOrganizationId { get; set; }

    [Display(Name = "Дата начала аренды")]
    [DataType(DataType.Date)]
    public DateTime CheckInDate { get; set; }

    [Display(Name = "Дата окончания аренды")]
    [DataType(DataType.Date)]
    [CheckOutDate("CheckInDate")]
    public DateTime CheckOutDate { get; set; }
    [ValidateNever]
    public virtual Organization RentalOrganization { get; set; }
    [ValidateNever]
    public virtual Room Room { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    [Display(Name = "Фотография")]
    public byte[] Photo { get; set; } = new byte[0];
    public virtual Building Building { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}

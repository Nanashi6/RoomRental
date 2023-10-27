using System;
using System.Collections.Generic;

namespace RoomRental.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public int BuildingId { get; set; }

    public decimal Area { get; set; }

    public string Description { get; set; } = null!;

    public byte[] Photo { get; set; } = null!;

    public virtual Building Building { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}

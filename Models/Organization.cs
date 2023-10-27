using System;
using System.Collections.Generic;

namespace RoomRental.Models;

public partial class Organization
{
    public int OrganizationId { get; set; }

    public string Name { get; set; } = null!;

    public string PostalAddress { get; set; } = null!;

    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}

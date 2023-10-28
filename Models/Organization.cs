using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoomRental.Models;

public partial class Organization
{
    [Display(Name = "Идентификатор")]
    public int OrganizationId { get; set; }
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;
    [Display(Name = "Почтовый адрес")]
    public string PostalAddress { get; set; } = null!;

    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}

using System;
using System.Collections.Generic;

namespace RoomRental.Models;

public partial class ResponsiblePerson
{
    public int PersonId { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}

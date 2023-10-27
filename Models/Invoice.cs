using System;
using System.Collections.Generic;

namespace RoomRental.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int? RentalOrganizationId { get; set; }

    public int RoomId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public int ResponsiblePerson { get; set; }

    public virtual Organization? RentalOrganization { get; set; }

    public virtual ResponsiblePerson ResponsiblePersonNavigation { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}

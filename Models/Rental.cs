using System;
using System.Collections.Generic;

namespace RoomRental.Models;

public partial class Rental
{
    public int RentalId { get; set; }

    public int RoomId { get; set; }

    public int RentalOrganizationId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public virtual Organization RentalOrganization { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}

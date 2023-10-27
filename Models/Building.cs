using System;
using System.Collections.Generic;

namespace RoomRental.Models;

public partial class Building
{
    public int BuildingId { get; set; }

    public string Name { get; set; } = null!;

    public int OwnerOrganizationId { get; set; }

    public string PostalAddress { get; set; } = null!;

    public int Floors { get; set; }

    public string Description { get; set; } = null!;

    public byte[] FloorPlan { get; set; } = null!;

    public virtual Organization OwnerOrganization { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}

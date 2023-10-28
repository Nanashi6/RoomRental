using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoomRental.Models;

public partial class BuildingBindModel
{
    public int BuildingId { get; set; }
    public string Name { get; set; }
    public int OwnerOrganizationId { get; set; }
    public string PostalAddress { get; set; }
    public int Floors { get; set; }
    public string Description { get; set; }
    public IFormFile FloorPlan { get; set; }
}

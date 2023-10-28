using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoomRental.Models;

public partial class RoomBindModel
{
    public int RoomId { get; set; }
    public int BuildingId { get; set; }
    public decimal Area { get; set; }
    public string Description { get; set; }
    public IFormFile Photo { get; set; }
}

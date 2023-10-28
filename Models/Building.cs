﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoomRental.Models;

public partial class Building
{
    [Display(Name = "Идентификатор")]
    public int BuildingId { get; set; }
    [Display(Name = "Название здания")]
    public string Name { get; set; }
    [Display(Name = "Организация-владелец")]
    public int OwnerOrganizationId { get; set; }
    [Display(Name = "Почтовый адрес")]
    public string PostalAddress { get; set; }
    [Display(Name = "Этажность")]
    public int Floors { get; set; }
    [Display(Name = "Описание")]
    public string Description { get; set; }
    [Display(Name = "План этажа")]
    public byte[] FloorPlan { get; set; }

    public virtual Organization OwnerOrganization { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}

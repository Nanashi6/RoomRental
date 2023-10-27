using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RoomRental.ViewModels
{
    public class RoomViewModel
    {
        [Display(Name = "Идентификатор")]
        public int RoomId { get; set; }

        [Display(Name = "Здание")]
        public string? Building { get; set; }

        [Display(Name = "Площадь")]
        public decimal? Area { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Фотография")]
        public FileContentResult? Photo { get; set; }
    }
}

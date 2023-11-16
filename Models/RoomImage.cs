using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace RoomRental.Models
{
    public class RoomImage
    {
        [Key]
        public int ImageId { get; set; }
        public string ImagePath { get; set; }
        public int RoomId { get; set; }
        [ValidateNever]
        public virtual Room Room { get; set; }
    }
}

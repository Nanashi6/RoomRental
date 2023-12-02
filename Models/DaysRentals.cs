using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomRental.Models
{
    public class DaysRentals
    {
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }
        public decimal Percentage { get; set; }
    }
}

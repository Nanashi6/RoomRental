using System.ComponentModel.DataAnnotations;

namespace RoomRental.ViewModels
{
    public class RentalViewModel
    {
        [Display(Name = "Идентификатор")]
        public int RentalId { get; set; }

        [Display(Name = "Комната")]
        public int RoomId { get; set; }

        [Display(Name = "Организация-арендатор")]
        public string RentalOrganization { get; set; }

        [Display(Name = "Дата начала аренды")]
        [DataType(DataType.Date)]
        public DateTime? CheckInDate { get; set; }

        [Display(Name = "Дата окончания аренды")]
        [DataType(DataType.Date)]
        public DateTime? CheckOutDate { get; set; }

        public RentalViewModel(int id, int room, string organization, DateTime? checkInDate, DateTime? checkOutDate)
        {
            RentalId = id;
            RoomId = room;
            RentalOrganization = organization;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
        }
    }
}

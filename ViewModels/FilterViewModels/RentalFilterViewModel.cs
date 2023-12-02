namespace RoomRental.ViewModels.FilterViewModels
{
    public class RentalFilterViewModel
    {
        public string OrganizationNameFind { get; set; }
        public int? BuildingIdFind { get; set; }
        public DateTime? CheckInDateStartFind { get; set; } = null;
        public DateTime? CheckInDateEndFind { get; set; } = null;
        public DateTime? CheckOutDateStartFind { get; set; } = null;
        public DateTime? CheckOutDateEndFind { get; set; } = null;

        public RentalFilterViewModel()
        {

        }
    }
}

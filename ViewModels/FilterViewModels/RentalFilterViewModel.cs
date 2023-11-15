namespace RoomRental.ViewModels.FilterViewModels
{
    public class RentalFilterViewModel
    {
        public string OrganizationNameFind { get; set; }
        public DateTime? CheckInDateFind { get; set; } = null;
        public DateTime? CheckOutDateFind { get; set; } = null;

        public RentalFilterViewModel()
        {

        }
        public RentalFilterViewModel(string name, DateTime? checkInDate, DateTime? checkOutDate)
        {
            OrganizationNameFind = name;
            CheckInDateFind = checkInDate;
            CheckOutDateFind = checkOutDate;
        }
    }
}

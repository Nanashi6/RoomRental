namespace RoomRental.ViewModels.FilterViewModels
{
    public class RentalFilterViewModel
    {
        public string OrganizationNameFind { get; set; }
        public DateTime? CheckInDateFind { get; set; }
        public DateTime? CheckOutDateFind { get; set; }

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

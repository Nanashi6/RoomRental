namespace RoomRental.ViewModels.FilterViewModels
{
    public class RentalFilterViewModel
    {
        public string OrganizationNameFind { get; }
        public DateTime? CheckInDateFind { get; }
        public DateTime? CheckOutDateFind { get; }

        public RentalFilterViewModel(string name, DateTime? checkInDate, DateTime? checkOutDate)
        {
            OrganizationNameFind = name;
            CheckInDateFind = checkInDate;
            CheckOutDateFind = checkOutDate;
        }
    }
}

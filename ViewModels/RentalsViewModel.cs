using RoomRental.ViewModels.FilterViewModels;

namespace RoomRental.ViewModels
{
    public class RentalsViewModel
    {
        public IEnumerable<RentalViewModel?> Rentals { get; set; }
        public PageViewModel? PageViewModel { get; set; }
        public RentalSortViewModel SortViewModel { get; set; }
        public RentalFilterViewModel FilterViewModel { get; set; }
    }
}

using RoomRental.Models;
using RoomRental.ViewModels.FilterViewModels;
using RoomRental.ViewModels.SortViewModels;

namespace RoomRental.ViewModels
{
    public class RentalsViewModel
    {
        public IEnumerable<Rental?> Rentals { get; set; }
        public PageViewModel? PageViewModel { get; set; }
        public RentalSortViewModel SortViewModel { get; set; }
        public RentalFilterViewModel FilterViewModel { get; set; }
    }
}

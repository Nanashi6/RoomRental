using RoomRental.Models;
using RoomRental.ViewModels.FilterViewModels;
using RoomRental.ViewModels.SortViewModels;

namespace RoomRental.ViewModels
{
    public class RoomsViewModel
    {
        public IEnumerable<Room?> Rooms { get; set; }
        public PageViewModel? PageViewModel { get; set; }
        public RoomFilterViewModel FilterViewModel { get; set; }
        public RoomSortViewModel SortViewModel { get; set; }
    }
}

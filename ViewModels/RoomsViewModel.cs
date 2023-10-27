using RoomRental.ViewModels.FilterViewModels;

namespace RoomRental.ViewModels
{
    public class RoomsViewModel
    {
        public IEnumerable<RoomViewModel?> Rooms { get; set; }
        public PageViewModel? PageViewModel { get; set; }
        public RoomFilterViewModel FilterViewModel { get; set; }
        public RoomSortViewModel SortViewModel { get; set; }
    }
}

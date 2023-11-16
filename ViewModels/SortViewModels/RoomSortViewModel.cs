using RoomRental.ViewModels.SortStates;

namespace RoomRental.ViewModels.SortViewModels
{
    public class RoomSortViewModel
    {
        public RoomSortState BuildingNameSort { get; set; }
        public RoomSortState AreaSort { get; set; }
        public RoomSortState Current { get; set; }
        public RoomSortViewModel(RoomSortState sortOrder)
        {
            BuildingNameSort = sortOrder == RoomSortState.BuildingNameAsc ? RoomSortState.BuildingNameDesc : RoomSortState.BuildingNameAsc;
            AreaSort = sortOrder == RoomSortState.AreaAsc ? RoomSortState.AreaDesc : RoomSortState.AreaAsc;
            Current = sortOrder;
        }
    }
}

using RoomRental.ViewModels.SortStates;

namespace RoomRental.ViewModels.SortViewModels
{
    public class RoomSortrViewModel
    {
        public BuildingSortState NameSort { get; set; }
        public BuildingSortState OrganizationNameSort { get; set; }
        public BuildingSortState AddressSort { get; set; }
        public BuildingSortState FloorsSort { get; set; }
        public BuildingSortState Current { get; set; }

        public RoomSortrViewModel(BuildingSortState sortOrder)
        {
            NameSort = sortOrder == BuildingSortState.NameAsc ? BuildingSortState.NameDesc : BuildingSortState.NameAsc;
            OrganizationNameSort = sortOrder == BuildingSortState.OrganizationNameAsc ? BuildingSortState.OrganizationNameDesc : BuildingSortState.OrganizationNameAsc;
            AddressSort = sortOrder == BuildingSortState.AddressAsc ? BuildingSortState.AddressDesc : BuildingSortState.AddressAsc;
            FloorsSort = sortOrder == BuildingSortState.FloorsAsc ? BuildingSortState.FloorsDesc : BuildingSortState.FloorsAsc;    
            Current = sortOrder;
        }
    }
}

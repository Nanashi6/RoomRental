using RoomRental.ViewModels.SortStates;

namespace RoomRental.ViewModels.SortViewModels
{
    public class OrganizationSortViewModel
    {
        public OrganizationSortState NameSort { get; set; }
        public OrganizationSortState AddressSort { get; set; }
        public OrganizationSortState Current { get; set; }

        public OrganizationSortViewModel(OrganizationSortState sortOrder)
        {
            NameSort = sortOrder == OrganizationSortState.NameAsc ? OrganizationSortState.NameDesc : OrganizationSortState.NameAsc;
            AddressSort = sortOrder == OrganizationSortState.AddressAsc ? OrganizationSortState.AddressDesc : OrganizationSortState.AddressAsc;
            Current = sortOrder;
        }
    }
}

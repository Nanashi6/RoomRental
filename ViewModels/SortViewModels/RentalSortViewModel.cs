using RoomRental.ViewModels.SortStates;

namespace RoomRental.ViewModels.SortViewModels
{
    public class RentalSortViewModel
    {
        public RentalSortState OrganizationNameSort {  get; set; }
        public RentalSortState CheckInDateSort { get; set; }
        public RentalSortState CheckOutDateSort { get; set; }
        public RentalSortState Current { get; set; }
        public RentalSortViewModel(RentalSortState sortOrder) 
        {
            OrganizationNameSort = sortOrder == RentalSortState.OrganizationNameAsc ? RentalSortState.OrganizationNameDesc : RentalSortState.OrganizationNameAsc;
            CheckInDateSort = sortOrder == RentalSortState.CheckInDateAsc ? RentalSortState.CheckInDateDesc : RentalSortState.CheckInDateAsc;
            CheckOutDateSort = sortOrder == RentalSortState.CheckOutDateAsc ? RentalSortState.CheckOutDateDesc : RentalSortState.CheckOutDateAsc;
            Current = sortOrder;
        }
    }
}

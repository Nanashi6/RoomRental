using RoomRental.ViewModels.SortStates;

namespace RoomRental.ViewModels.SortViewModels
{
    public class PersonSortViewModel
    {
        public PersonSortState SurnameSort {  get; set; }
        public PersonSortState NameSort {  get; set; }
        public PersonSortState LastnameSort { get; set; }
        public PersonSortState Current {  get; set; }
        public PersonSortViewModel(PersonSortState sortOrder)
        {
            SurnameSort = sortOrder == PersonSortState.SurnameAsc ? PersonSortState.SurnameDesc : PersonSortState.SurnameAsc;
            NameSort = sortOrder == PersonSortState.NameAsc ? PersonSortState.NameDesc : PersonSortState.NameAsc;
            LastnameSort = sortOrder == PersonSortState.LastnameAsc ? PersonSortState.LastnameDesc : PersonSortState.LastnameAsc;
            Current = sortOrder;
        }
    }
}

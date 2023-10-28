using RoomRental.Models;
using RoomRental.ViewModels.FilterViewModels;
using RoomRental.ViewModels.SortViewModels;

namespace RoomRental.ViewModels
{
    public class PeopleViewModel
    {
        public IEnumerable<ResponsiblePerson?> People { get; set; }
        public PageViewModel? PageViewModel { get; set; }
        public PersonSortViewModel SortViewModel { get; set; }
        public PersonFilterViewModel FilterViewModel { get; set; }
    }
}

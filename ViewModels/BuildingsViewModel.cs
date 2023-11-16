using RoomRental.Models;
using RoomRental.ViewModels.FilterViewModels;
using RoomRental.ViewModels.SortViewModels;

namespace RoomRental.ViewModels
{
    public class BuildingsViewModel
    {
        public IEnumerable<Building>? Buildings { get; set; }
        public PageViewModel? PageViewModel { get; set; }
        public RoomSortrViewModel? SortViewModel { get; set; }
        public BuildingFilterViewModel FilterViewModel { get; set; }
    }
}

using RoomRental.ViewModels.FilterViewModels;

namespace RoomRental.ViewModels
{
    public class BuildingsViewModel
    {
        public IEnumerable<BuildingViewModel>? Buildings { get; set; }
        public PageViewModel? PageViewModel { get; set; }
        public string? BuildingNameFind { get; set; }
        public BuildingSortViewModel? SortViewModel { get; set; }
        public BuildingFilterViewModel FilterViewModel { get; set; }
    }
}

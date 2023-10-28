using RoomRental.Models;
using RoomRental.ViewModels.SortViewModels;

namespace RoomRental.ViewModels
{
    public class OrganizationsViewModel
    {
        public IEnumerable<Organization>? Organizations { get; set; }
        public PageViewModel? PageViewModel { get; set; }
        public string? OrganizationNameFind { get; set; }
        public OrganizationSortViewModel? SortViewModel { get; set; }
    }
}

namespace RoomRental.ViewModels.FilterViewModels
{
    public class BuildingFilterViewModel
    {
        public string BuildingNameFind { get; set; }
        public string OrganizationNameFind { get; set; }
        public string AddressFind { get; set; }
        public int? FloorsFind { get; set; }

        public BuildingFilterViewModel()
        {
            
        }
        public BuildingFilterViewModel(string name, string organizationName, string address, int? floors)
        {
            BuildingNameFind = name;
            OrganizationNameFind = organizationName;
            AddressFind = address;
            FloorsFind = floors;
        }
    }
}

namespace RoomRental.ViewModels.FilterViewModels
{
    public class BuildingFilterViewModel
    {
        public string BuildingNameFind { get; }
        public string OrganizationNameFind { get; }
        public string AddressFind { get; }
        public int? FloorsFind { get; }

        public BuildingFilterViewModel(string name, string organizationName, string address, int? floors)
        {
            BuildingNameFind = name;
            OrganizationNameFind = organizationName;
            AddressFind = address;
            FloorsFind = floors;
        }
    }
}

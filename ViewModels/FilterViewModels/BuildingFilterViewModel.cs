namespace RoomRental.ViewModels.FilterViewModels
{
    public class BuildingFilterViewModel
    {
        public string NameFind { get; }
        public string OrganizationNameFind { get; }
        public string AddressFind { get; }
        public int? FloorsFind { get; }

        public BuildingFilterViewModel(string name, string organizationName, string address, int? floors)
        {
            NameFind = name;
            OrganizationNameFind = organizationName;
            AddressFind = address;
            FloorsFind = floors;
        }
    }
}

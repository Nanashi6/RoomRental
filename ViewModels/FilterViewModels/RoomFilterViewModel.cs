namespace RoomRental.ViewModels.FilterViewModels
{
    public class RoomFilterViewModel
    {
        public string BuildingNameFind { get; set; }
        public decimal? AreaFind { get; set; }

        public RoomFilterViewModel()
        {

        }
        public RoomFilterViewModel(string name, decimal? area)
        {
            BuildingNameFind = name;
            AreaFind = area;
        }
    }
}

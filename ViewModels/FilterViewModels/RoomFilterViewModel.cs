namespace RoomRental.ViewModels.FilterViewModels
{
    public class RoomFilterViewModel
    {
        public string BuildingNameFind { get; }
        public decimal? AreaFind { get; }

        public RoomFilterViewModel(string name, decimal? area)
        {
            BuildingNameFind = name;
            AreaFind = area;
        }
    }
}

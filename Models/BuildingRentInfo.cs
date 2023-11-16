namespace RoomRental.Models
{
    public class BuildingRentInfo
    {
        public decimal TotalArea { get; set; }
        public decimal RentedArea { get; set; }
        public decimal RentPercentage { get; set; }
        public List<Organization> RentingOrganizations { get; set; }
    }
}

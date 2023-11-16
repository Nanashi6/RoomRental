using Microsoft.AspNetCore.Identity;

namespace RoomRental.Models
{
    public class User : IdentityUser
    {
        public string Surname { get; set; }
        public string Name { get; set; }
    }
}

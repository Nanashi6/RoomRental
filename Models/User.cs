using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace RoomRental.Models
{
    [Keyless]
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}

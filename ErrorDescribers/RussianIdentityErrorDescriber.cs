using Microsoft.AspNetCore.Identity;

namespace RoomRental.ErrorDescribers
{
    public class RussianIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = "Такой логин уже занят"
            };
        }
    }
}

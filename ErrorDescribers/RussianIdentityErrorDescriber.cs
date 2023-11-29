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

		public override IdentityError DuplicateEmail(string email)
		{
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = "Такой email уже занят"
            };                
		}

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = "Пароль слишком короткий"
            };
        }

    }
}

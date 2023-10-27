namespace RoomRental.ViewModels.FilterViewModels
{
    public class PersonFilterViewModel
    {
        public string SurnameFind { get; }
        public string NameFind { get; }
        public string LastnameFind { get; }

        public PersonFilterViewModel(string surname, string name, string lastname)
        {
            SurnameFind = surname;
            NameFind = name;
            LastnameFind = lastname;
        }
    }
}

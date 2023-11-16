namespace RoomRental.ViewModels.FilterViewModels
{
    public class PersonFilterViewModel
    {
        public string SurnameFind { get; set; }
        public string NameFind { get; set; }
        public string LastnameFind { get; set; }

        public PersonFilterViewModel()
        {

        }
        public PersonFilterViewModel(string surname, string name, string lastname)
        {
            SurnameFind = surname;
            NameFind = name;
            LastnameFind = lastname;
        }
    }
}

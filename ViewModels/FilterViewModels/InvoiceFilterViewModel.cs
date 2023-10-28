namespace RoomRental.ViewModels.FilterViewModels
{
    public class InvoiceFilterViewModel
    {
        public string OrganizationNameFind { get; }
        public string ResponsiblePersonFind { get; }
        public decimal? AmountFind { get; }
        public DateTime? PaymentDateFind { get; }

        public InvoiceFilterViewModel(string name, DateTime? date, decimal? amount, string person)
        {
            OrganizationNameFind = name;
            PaymentDateFind = date;
            AmountFind = amount;
            ResponsiblePersonFind = person;
        }
    }
}

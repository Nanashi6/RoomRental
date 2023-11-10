namespace RoomRental.ViewModels.FilterViewModels
{
    public class InvoiceFilterViewModel
    {
        public string OrganizationNameFind { get; set; }
        public string ResponsiblePersonFind { get; set; }
        public decimal? AmountFind { get; set; }
        public DateTime? PaymentDateFind { get; set; }

        public InvoiceFilterViewModel()
        {
            
        }
        public InvoiceFilterViewModel(string name, DateTime? date, decimal? amount, string person)
        {
            OrganizationNameFind = name;
            PaymentDateFind = date;
            AmountFind = amount;
            ResponsiblePersonFind = person;
        }
    }
}

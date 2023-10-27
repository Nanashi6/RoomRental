using RoomRental.ViewModels.FilterViewModels;

namespace RoomRental.ViewModels
{
    public class InvoicesViewModel
    {
        public IEnumerable<InvoiceViewModel?> Invoices { get; set; }
        public PageViewModel? PageViewModel { get; set; }
        public InvoiceFilterViewModel FilterViewModel { get; set; }
        public InvoiceSortViewModel SortViewModel { get; set; }
    }
}

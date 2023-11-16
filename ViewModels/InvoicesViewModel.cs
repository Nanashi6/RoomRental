using RoomRental.Models;
using RoomRental.ViewModels.FilterViewModels;
using RoomRental.ViewModels.SortViewModels;

namespace RoomRental.ViewModels
{
    public class InvoicesViewModel
    {
        public IEnumerable<Invoice?> Invoices { get; set; }
        public PageViewModel? PageViewModel { get; set; }
        public InvoiceFilterViewModel FilterViewModel { get; set; }
        public InvoiceSortViewModel SortViewModel { get; set; }
    }
}

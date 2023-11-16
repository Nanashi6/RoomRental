using RoomRental.ViewModels.SortStates;

namespace RoomRental.ViewModels.SortViewModels
{
    public class InvoiceSortViewModel
    {
        public InvoiceSortState OrganizationNameSort { get; set; }
        public InvoiceSortState PaymentDateSort { get; set; }
        public InvoiceSortState AmountSort { get; set; }
        public InvoiceSortState ResponsiblePersonSort { get; set; }
        public InvoiceSortState Current { get; set; }
        public InvoiceSortViewModel(InvoiceSortState sortOrder)
        {
            OrganizationNameSort = sortOrder == InvoiceSortState.OrganizationNameAsc ? InvoiceSortState.OrganizationNameDesc : InvoiceSortState.OrganizationNameAsc;
            PaymentDateSort = sortOrder == InvoiceSortState.PaymentDateAsc ? InvoiceSortState.PaymentDateDesc : InvoiceSortState.PaymentDateAsc;
            AmountSort = sortOrder == InvoiceSortState.AmountAsc ? InvoiceSortState.AmountDesc : InvoiceSortState.AmountAsc;
            ResponsiblePersonSort = sortOrder == InvoiceSortState.ResponsiblePersonAsc ? InvoiceSortState.ResponsiblePersonDesc : InvoiceSortState.ResponsiblePersonAsc;
            Current = sortOrder;
        }
    }
}

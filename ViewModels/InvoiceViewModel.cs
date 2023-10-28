using System.ComponentModel.DataAnnotations;

namespace RoomRental.ViewModels
{
    public class InvoiceViewModel
    {
        [Display(Name = "Идентификатор")]
        public int InvoiceId { get; set; }
        [Display(Name = "Организация-арендатор")]
        public string RentalOrganization { get; set; }
        [Display(Name = "Помещение")]
        public int RoomId { get; set; }
        [Display(Name = "Сумма оплаты")]
        public decimal Amount { get; set; }
        [Display(Name = "Дата оплаты")]
        public DateTime PaymentDate { get; set; }
        [Display(Name = "Оформляющий")]
        public string ResponsiblePerson { get; set; }
        public InvoiceViewModel(int id, string organization, int roomId, decimal amount, DateTime date, string person)
        {
            InvoiceId = id;
            RentalOrganization = organization;
            RoomId = roomId;
            Amount = amount;
            PaymentDate = date;
            ResponsiblePerson = person;
        }
    }
}

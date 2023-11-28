using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;

namespace RoomRental.Models;

public partial class Invoice
{
    [Display(Name = "Идентификатор")]
    public int InvoiceId { get; set; }

    [Required(ErrorMessage = "Не указана организация-арендатор")]
    [Display(Name = "Организация-арендатор")]
    public int RentalOrganizationId { get; set; }

    [Required(ErrorMessage = "Не указано помещение")]
    [Display(Name = "Помещение")]
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Не указана сумма оплаты")]
    [Display(Name = "Сумма оплаты")]
    [Range(0, double.MaxValue, ErrorMessage = "Значение не может быть меньше нуля")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Не указана дата оплаты")]
    [Display(Name = "Дата оплаты")]
    [DataType(DataType.Date)]
    public DateTime PaymentDate { get; set; }

    [Required(ErrorMessage = "Не указан оформляющий")]
    [Display(Name = "Оформляющий")]
    public int ResponsiblePersonId { get; set; }

    [Display(Name = "Организация-арендатор")]
    [ValidateNever]
    public virtual Organization? RentalOrganization { get; set; }

    [Display(Name = "Оформляющий")]
    [ValidateNever]
    public virtual ResponsiblePerson? ResponsiblePerson { get; set; } = null!;

    [ValidateNever]
    public virtual Room? Room { get; set; } = null!;
}

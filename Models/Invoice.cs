﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace RoomRental.Models;

public partial class Invoice
{
    [Display(Name = "Идентификатор")]
    public int InvoiceId { get; set; }
    [Display(Name = "Организация-арендатор")]
    public int? RentalOrganizationId { get; set; }
    [Display(Name = "Помещение")]
    public int RoomId { get; set; }
    [Display(Name = "Сумма оплаты")]
    public decimal Amount { get; set; }
    [Display(Name = "Дата оплаты")]
    [DataType(DataType.Date)]
    public DateTime PaymentDate { get; set; }
    [Display(Name = "Оформляющий")]
    public int ResponsiblePerson { get; set; }

    public virtual Organization? RentalOrganization { get; set; }

    public virtual ResponsiblePerson? ResponsiblePersonNavigation { get; set; } = null!;

    public virtual Room? Room { get; set; } = null!;
}

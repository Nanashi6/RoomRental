using System;
using System.ComponentModel.DataAnnotations;

namespace RoomRental.Attributes
{
    public class CheckOutDateAttribute : ValidationAttribute
    {
        private readonly string _startDatePropertyName;

        public CheckOutDateAttribute(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            if (startDateProperty == null)
            {
                return new ValidationResult($"Unknown property: {_startDatePropertyName}");
            }

            var startDateValue = (DateTime)startDateProperty.GetValue(validationContext.ObjectInstance);
            var endDateValue = (DateTime)value;

            if (endDateValue < startDateValue)
            {
                return new ValidationResult("Дата окончания аренды не может быть меньше даты начала аренды.");
            }
            else if (endDateValue == startDateValue) 
            {
                return new ValidationResult("Дата окончания аренды не может быть равна дате начала аренды.");
            }

            return ValidationResult.Success;
        }
    }
}

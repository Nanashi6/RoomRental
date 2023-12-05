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

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            if (startDateProperty == null)
            {
                return new ValidationResult($"Неизвестное значение: {_startDatePropertyName}");
            }

            var startDateValue = (DateTime?)startDateProperty.GetValue(validationContext.ObjectInstance);
            var endDateValue = (DateTime?)value;

            if (endDateValue < startDateValue)
            {
                return new ValidationResult($"Дата не может быть меньше {startDateValue}");
            }
            else if (endDateValue == startDateValue)
            {
                return new ValidationResult($"Дата не может быть равна {startDateValue}");
            }

            return ValidationResult.Success;
        }
    }
}

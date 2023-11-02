using Microsoft.AspNetCore.Mvc.ModelBinding;
using NuGet.Protocol;
using RoomRental.Models;
using System.Text;

namespace RoomRental
{
    public class BuildingBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // в случае ошибки возвращаем исключение
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // с помощью поставщика значений получаем данные из запроса
            var buildingIdValues = bindingContext.ValueProvider.GetValue("BuildingId");
            var nameValues = bindingContext.ValueProvider.GetValue("Name");
            var ownerOrganizationIdValues = bindingContext.ValueProvider.GetValue("OwnerOrganizationId");
            var postalAddressValues = bindingContext.ValueProvider.GetValue("PostalAddress");
            var floorsValues = bindingContext.ValueProvider.GetValue("Floors");
            var descriptionValues = bindingContext.ValueProvider.GetValue("Description");
            var floorPlanValues = bindingContext.ValueProvider.GetValue("FloorPlan");

            // получаем значения
            string buildingId = buildingIdValues.FirstValue;
            string name = nameValues.FirstValue;
            string ownerOrganizationId = ownerOrganizationIdValues.FirstValue;
            string postalAddress = postalAddressValues.FirstValue;
            string floors = floorsValues.FirstValue;
            string description = descriptionValues.FirstValue;
            string floorPlan = floorPlanValues.ToJson();

            // если id не установлен, например, при создании модели, генерируем его
            if (String.IsNullOrEmpty(buildingId)) buildingId = Guid.NewGuid().ToString();

            // устанавливаем результат привязки
            bindingContext.Result = ModelBindingResult.Success(new Building
            {
                BuildingId = Int32.Parse(buildingId),
                Name = name,
                OwnerOrganizationId = Int32.Parse(ownerOrganizationId),
                PostalAddress = postalAddress,
                Description = description,
                Floors = Int32.Parse(floors),
                FloorPlan = new byte[0]
            });
            return Task.CompletedTask;
        }
    }
}

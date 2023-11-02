using Microsoft.AspNetCore.Mvc.ModelBinding;
using RoomRental.Models;

namespace RoomRental
{
    public class BuildingBindProvider : IModelBinderProvider
    {
        private readonly IModelBinder binder = new BuildingBinder();

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return context.Metadata.ModelType == typeof(Building) ? binder : null;
        }
    }
}

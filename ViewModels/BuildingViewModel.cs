using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RoomRental.ViewModels
{
    public class BuildingViewModel
    {
        [Display(Name = "Идентификатор")]
        public int BuildingId { get; set; }
        [Display(Name = "Название здания")]
        public string Name { get; set; }
        [Display(Name = "Организация-владелец")]
        public string OwnerOrganization { get; set; }
        [Display(Name = "Почтовый адрес")]
        public string PostalAddress { get; set; }
        [Display(Name = "Этажность")]
        public int Floors { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "План этажа")]
        public FileContentResult FloorPlan { get; set; }

        public BuildingViewModel(int id, string name, string organization, string address, int floors, string description, FileContentResult plan)
        {
            BuildingId = id;
            Name = name;
            OwnerOrganization = organization;
            PostalAddress = address;
            Floors = floors;
            Description = description;
            FloorPlan = plan;
        }
    }
}

using RoomRental.Models;
using System;

namespace RoomRental.Data
{
    public static class DbInizializer
    {
        public static void Inizialize(RoomRentalsContext context)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            if (!context.Organizations.Any())
            {
                for (int i = 0; i < 100; i++)
                {
                    context.Add(new Organization() { Name = $"Organization {i + 1}", PostalAddress = $"Address {i + 1}" });
                }
                context.SaveChanges();
            }

            if (!context.Buildings.Any())
            {
                for (int i = 0; i < 500; i++)
                {
                    int ownerId = rnd.Next(context.Organizations.Count()) + 1;
                    context.Buildings.Add(new Building() { Name = $"Building {i + 1}", PostalAddress = $"Address {i + 1}", Floors = rnd.Next(11), Description = $"Description {i + 1}", OwnerOrganizationId = ownerId, FloorPlan = $"\\Images\\FloorPlans\\1.jpg", OwnerOrganization = context.Organizations.ToArray()[ownerId-1] });
                }
                context.SaveChanges();
            }

            if (!context.Rooms.Any())
            {
                for (int i = 0; i < 10000; i++)
                {
                    int buildingId = rnd.Next(context.Buildings.Count()) + 1;
                    context.Add(new Room() { Area = rnd.Next(100), BuildingId = buildingId, Description = $"Description {i + 1}", Building = context.Buildings.ToArray()[buildingId-1] });
                }
                context.SaveChanges();
            }

            if (!context.RoomImages.Any())
            {
                for (int i = 0; i < 10000; i++)
                {
                    int roomId = i + 1;
                    context.Add(new RoomImage() { RoomId = roomId, ImagePath = "\\Images\\Rooms\\1.jpg", Room = context.Rooms.ToArray()[roomId-1] });
                }
                context.SaveChanges();
            }

            if (!context.ResponsiblePeople.Any())
            {
                for (int i = 0; i < 100; i++)
                {
                    context.Add(new ResponsiblePerson() { Surname = $"Surname {i+1}", Name = $"Name {i+1}", Lastname = $"Lastname {i+1}" });
                }
                context.SaveChanges();
            }

            if (!context.Rentals.Any())
            {
                for (int i = 0; i < 10000; i++)
                {
                    int roomId = rnd.Next(context.Rooms.Count()) + 1;
                    int rentalOrgId = rnd.Next(context.Organizations.Count()) + 1;
                    DateTime checkInDate = GenerateRandomDate();
                    DateTime checkOutDate = GenerateRandomDate(checkInDate);

                    context.Add(new Rental() { RoomId = roomId, RentalOrganizationId = rentalOrgId, CheckInDate = checkInDate, CheckOutDate = checkOutDate, Room = context.Rooms.ToArray()[roomId-1], RentalOrganization = context.Organizations.ToArray()[rentalOrgId-1] });
                }
                context.SaveChanges();
            }

            if (!context.Invoices.Any())
            {
                for (int i = 0; i < 10000; i++)
                {
                    int roomId = rnd.Next(context.Rooms.Count()) + 1;
                    int rentalOrgId = rnd.Next(context.Organizations.Count()) + 1;
                    int responsiblePersonId = rnd.Next(context.ResponsiblePeople.Count()) + 1;
                    DateTime paymentDate = GenerateRandomDate();

                    context.Add(new Invoice() { RoomId = roomId, RentalOrganizationId = rentalOrgId, Amount = rnd.Next(10000), ResponsiblePersonId = responsiblePersonId, PaymentDate = paymentDate, Room = context.Rooms.ToArray()[roomId - 1], RentalOrganization = context.Organizations.ToArray()[rentalOrgId - 1], ResponsiblePerson = context.ResponsiblePeople.ToArray()[responsiblePersonId-1] });
                }
                context.SaveChanges();
            }
        }  
        private static DateTime GenerateRandomDate(DateTime? minDate = null, DateTime? maxDate = null)
        {
            Random random = new(Guid.NewGuid().GetHashCode());
            DateTime startDate = minDate ?? new DateTime(2020, 1, 1);
            DateTime endDate = maxDate ?? new DateTime(2024, 12, 31);

            TimeSpan timeSpan = endDate - startDate;
            TimeSpan randomSpan = new TimeSpan((long)(random.NextDouble() * timeSpan.Ticks));
            DateTime randomDate = startDate + randomSpan;

            return randomDate;
        }
    }
}

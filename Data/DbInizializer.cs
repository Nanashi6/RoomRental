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

            var organizations = context.Organizations.ToArray();
            if (!context.Buildings.Any())
            {
                for (int i = 0; i < 500; i++)
                {
                    int ownerId = rnd.Next(organizations.Count()) + 1;
                    context.Buildings.Add(new Building() { Name = $"Building {i + 1}", PostalAddress = $"Address {i + 1}", Floors = rnd.Next(11), Description = $"Description {i + 1}", OwnerOrganizationId = ownerId, FloorPlan = $"\\Images\\FloorPlans\\1.jpg", OwnerOrganization = organizations[ownerId-1] });
                }
                context.SaveChanges();
            }

            var buildings = context.Buildings.ToArray();
            if (!context.Rooms.Any())
            {

                for (int i = 1; i <= buildings.Length; i++)
                {
                    for (int j = 0; j < 10; j++)
                        context.Add(new Room() { Area = rnd.Next(50,100), BuildingId = i, RoomNumber = j+1, Description = $"Description {(i-1) * 10 + (j + 1)}", Building = context.Buildings.ToArray()[i-1] });
                }

                context.SaveChanges();
            }

            var rooms = context.Rooms.ToArray();
            if (!context.RoomImages.Any())
            {
                for (int i = 0; i < 5000; i++)
                {
                    int roomId = i + 1;
                    context.Add(new RoomImage() { RoomId = roomId, ImagePath = "\\Images\\Rooms\\1.jpg", Room = rooms[roomId-1] });
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

            var people = context.ResponsiblePeople.ToArray();
            if (!context.Rentals.Any())
            {
                for (int i = 0; i < 10000; i++)
                {
                    int roomId = rnd.Next(rooms.Count()) + 1;
                    int rentalOrgId = rnd.Next(organizations.Count()) + 1;
                    DateTime checkInDate = GenerateRandomDate();
                    DateTime checkOutDate = GenerateRandomDate(checkInDate, checkInDate.AddMonths(1));
                    int responsiblePersonId = rnd.Next(people.Count()) + 1;
                    DateTime paymentDate = GenerateRandomDate(checkInDate, checkInDate.AddDays(7));

                    context.Add(new Rental() { RoomId = roomId, RentalOrganizationId = rentalOrgId, CheckInDate = checkInDate, CheckOutDate = checkOutDate, Room = rooms[roomId-1], RentalOrganization = organizations[rentalOrgId-1] });
                    context.Add(new Invoice() { RoomId = roomId, RentalOrganizationId = rentalOrgId, ConclusionDate = checkInDate, Amount = rnd.Next(3000,5000), ResponsiblePersonId = responsiblePersonId, PaymentDate = paymentDate, Room = rooms[roomId - 1], RentalOrganization = organizations[rentalOrgId - 1], ResponsiblePerson = people[responsiblePersonId - 1] });
                }
                context.SaveChanges();
            }
        }  
        private static DateTime GenerateRandomDate(DateTime? minDate = null, DateTime? maxDate = null)
        {
            Random random = new(Guid.NewGuid().GetHashCode());
            DateTime startDate = minDate ?? new DateTime(2021, 1, 1);
            DateTime endDate = maxDate ?? new DateTime(2023, 12, 31);

            TimeSpan timeSpan = endDate - startDate;
            TimeSpan randomSpan = new TimeSpan((long)(random.NextDouble() * timeSpan.Ticks));
            DateTime randomDate = startDate + randomSpan;

            return randomDate;
        }
    }
}

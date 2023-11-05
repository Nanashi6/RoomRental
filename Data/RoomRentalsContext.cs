using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoomRental.Models;

namespace RoomRental.Data;

public partial class RoomRentalsContext : IdentityDbContext<IdentityUser>
{
    public RoomRentalsContext()
    {
    }

    public RoomRentalsContext(DbContextOptions<RoomRentalsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Rental> Rentals { get; set; }
    public virtual DbSet<ResponsiblePerson> ResponsiblePeople { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomImage> RoomImages { get; set; }

/*    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;Database=RoomRentals;Trusted_Connection=True;TrustServerCertificate=True;");*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.BuildingId).HasName("PK__Building__979FD1CD67568AE3");

            entity.Property(e => e.BuildingId).HasColumnName("buildingId");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.FloorPlan)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("floorPlan");
            entity.Property(e => e.Floors).HasColumnName("floors");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.OwnerOrganizationId).HasColumnName("ownerOrganizationId");
            entity.Property(e => e.PostalAddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("postalAddress");

            entity.HasOne(d => d.OwnerOrganization).WithMany(p => p.Buildings)
                .HasForeignKey(d => d.OwnerOrganizationId)
                .HasConstraintName("FK__Buildings__owner__398D8EEE");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoices__1252416CB6BF50FA");

            entity.Property(e => e.InvoiceId).HasColumnName("invoiceId");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.PaymentDate)
                .HasColumnType("date")
                .HasColumnName("paymentDate");
            entity.Property(e => e.RentalOrganizationId).HasColumnName("rentalOrganizationId");
            entity.Property(e => e.ResponsiblePerson).HasColumnName("responsiblePerson");
            entity.Property(e => e.RoomId).HasColumnName("roomId");

            entity.HasOne(d => d.RentalOrganization).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.RentalOrganizationId)
                .HasConstraintName("FK__Invoices__rental__44FF419A");

            entity.HasOne(d => d.ResponsiblePersonNavigation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.ResponsiblePerson)
                .HasConstraintName("FK__Invoices__respon__46E78A0C");

            entity.HasOne(d => d.Room).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoices__roomId__45F365D3");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.OrganizationId).HasName("PK__Organiza__29747D592CC83A43");

            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PostalAddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("postalAddress");
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(e => e.RentalId).HasName("PK__Rentals__0164732E677AD17B");

            entity.Property(e => e.RentalId).HasColumnName("rentalId");
            entity.Property(e => e.CheckInDate)
                .HasColumnType("date")
                .HasColumnName("checkInDate");
            entity.Property(e => e.CheckOutDate)
                .HasColumnType("date")
                .HasColumnName("checkOutDate");
            entity.Property(e => e.RentalOrganizationId).HasColumnName("rentalOrganizationId");
            entity.Property(e => e.RoomId).HasColumnName("roomId");

            entity.HasOne(d => d.RentalOrganization).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.RentalOrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rentals__rentalO__403A8C7D");

            entity.HasOne(d => d.Room).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rentals__roomId__3F466844");
        });

        modelBuilder.Entity<ResponsiblePerson>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__Responsi__EC7D7D4DDE04AF9B");

            entity.Property(e => e.PersonId).HasColumnName("personId");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lastname");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("surname");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Rooms__6C3BF5BE42D59D2F");

            entity.Property(e => e.RoomId).HasColumnName("roomId");
            entity.Property(e => e.Area)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("area");
            entity.Property(e => e.BuildingId).HasColumnName("buildingId");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");

            entity.HasOne(d => d.Building).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("FK__Rooms__buildingI__3C69FB99");
        });

        modelBuilder.Entity<RoomImage>(entity =>
        {
            entity.HasKey(e => e.ImageId);

            entity.Property(e => e.ImagePath)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("imagePath");

            entity.HasIndex(e => e.RoomId, "IX_RoomImages_RoomId");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomImages).HasForeignKey(d => d.RoomId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

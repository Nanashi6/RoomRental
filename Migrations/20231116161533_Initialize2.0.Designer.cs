﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoomRental.Data;

#nullable disable

namespace RoomRental.Migrations
{
    [DbContext(typeof(RoomRentalsContext))]
    [Migration("20231116161533_Initialize2.0")]
    partial class Initialize20
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RoomRental.Models.Building", b =>
                {
                    b.Property<int>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("buildingId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BuildingId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("description");

                    b.Property<string>("FloorPlan")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("floorPlan");

                    b.Property<int>("Floors")
                        .HasColumnType("int")
                        .HasColumnName("floors");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.Property<int>("OwnerOrganizationId")
                        .HasColumnType("int")
                        .HasColumnName("ownerOrganizationId");

                    b.Property<string>("PostalAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("postalAddress");

                    b.HasKey("BuildingId")
                        .HasName("PK__Building__979FD1CD3DCCFF84");

                    b.HasIndex("OwnerOrganizationId");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("RoomRental.Models.Invoice", b =>
                {
                    b.Property<int>("InvoiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("invoiceId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InvoiceId"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("amount");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("date")
                        .HasColumnName("paymentDate");

                    b.Property<int>("RentalOrganizationId")
                        .HasColumnType("int")
                        .HasColumnName("rentalOrganizationId");

                    b.Property<int>("ResponsiblePersonId")
                        .HasColumnType("int")
                        .HasColumnName("responsiblePersonId");

                    b.Property<int>("RoomId")
                        .HasColumnType("int")
                        .HasColumnName("roomId");

                    b.HasKey("InvoiceId")
                        .HasName("PK__Invoices__DDA6423A7D292366");

                    b.HasIndex("RentalOrganizationId");

                    b.HasIndex("ResponsiblePersonId");

                    b.HasIndex("RoomId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("RoomRental.Models.Organization", b =>
                {
                    b.Property<int>("OrganizationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("organizationId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrganizationId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.Property<string>("PostalAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("postalAddress");

                    b.HasKey("OrganizationId")
                        .HasName("PK__Organiza__29747D5940E6BB82");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("RoomRental.Models.Rental", b =>
                {
                    b.Property<int>("RentalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("rentalId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RentalId"));

                    b.Property<DateTime>("CheckInDate")
                        .HasColumnType("date")
                        .HasColumnName("checkInDate");

                    b.Property<DateTime>("CheckOutDate")
                        .HasColumnType("date")
                        .HasColumnName("checkOutDate");

                    b.Property<int>("RentalOrganizationId")
                        .HasColumnType("int")
                        .HasColumnName("rentalOrganizationId");

                    b.Property<int>("RoomId")
                        .HasColumnType("int")
                        .HasColumnName("roomId");

                    b.HasKey("RentalId")
                        .HasName("PK__Rentals__1D4A79C91609EF5D");

                    b.HasIndex("RentalOrganizationId");

                    b.HasIndex("RoomId");

                    b.ToTable("Rentals");
                });

            modelBuilder.Entity("RoomRental.Models.ResponsiblePerson", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("personId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonId"));

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("lastname");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("surname");

                    b.HasKey("PersonId")
                        .HasName("PK__Responsi__EC7D7D4D5E9ADA9C");

                    b.ToTable("ResponsiblePeople");
                });

            modelBuilder.Entity("RoomRental.Models.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("roomId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoomId"));

                    b.Property<decimal>("Area")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("area");

                    b.Property<int>("BuildingId")
                        .HasColumnType("int")
                        .HasColumnName("buildingId");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("description");

                    b.HasKey("RoomId")
                        .HasName("PK__Rooms__6C3BF5BEA3A3945D");

                    b.HasIndex("BuildingId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("RoomRental.Models.RoomImage", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("imageId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImageId"));

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("imagePath");

                    b.Property<int>("RoomId")
                        .HasColumnType("int")
                        .HasColumnName("roomId");

                    b.HasKey("ImageId")
                        .HasName("PK__RoomImag__336E9B55CAABD719");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomImages");
                });

            modelBuilder.Entity("RoomRental.Models.Building", b =>
                {
                    b.HasOne("RoomRental.Models.Organization", "OwnerOrganization")
                        .WithMany("Buildings")
                        .HasForeignKey("OwnerOrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Buildings__owner__37703C52");

                    b.Navigation("OwnerOrganization");
                });

            modelBuilder.Entity("RoomRental.Models.Invoice", b =>
                {
                    b.HasOne("RoomRental.Models.Organization", "RentalOrganization")
                        .WithMany("Invoices")
                        .HasForeignKey("RentalOrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Invoices__rental__42E1EEFE");

                    b.HasOne("RoomRental.Models.ResponsiblePerson", "ResponsiblePerson")
                        .WithMany("Invoices")
                        .HasForeignKey("ResponsiblePersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Invoices__respon__44CA3770");

                    b.HasOne("RoomRental.Models.Room", "Room")
                        .WithMany("Invoices")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Invoices__roomId__43D61337");

                    b.Navigation("RentalOrganization");

                    b.Navigation("ResponsiblePerson");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("RoomRental.Models.Rental", b =>
                {
                    b.HasOne("RoomRental.Models.Organization", "RentalOrganization")
                        .WithMany("Rentals")
                        .HasForeignKey("RentalOrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Rentals__rentalO__3E1D39E1");

                    b.HasOne("RoomRental.Models.Room", "Room")
                        .WithMany("Rentals")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Rentals__roomId__3D2915A8");

                    b.Navigation("RentalOrganization");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("RoomRental.Models.Room", b =>
                {
                    b.HasOne("RoomRental.Models.Building", "Building")
                        .WithMany("Rooms")
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Rooms__buildingI__3A4CA8FD");

                    b.Navigation("Building");
                });

            modelBuilder.Entity("RoomRental.Models.RoomImage", b =>
                {
                    b.HasOne("RoomRental.Models.Room", "Room")
                        .WithMany("RoomImages")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__RoomImage__roomI__47A6A41B");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("RoomRental.Models.Building", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("RoomRental.Models.Organization", b =>
                {
                    b.Navigation("Buildings");

                    b.Navigation("Invoices");

                    b.Navigation("Rentals");
                });

            modelBuilder.Entity("RoomRental.Models.ResponsiblePerson", b =>
                {
                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("RoomRental.Models.Room", b =>
                {
                    b.Navigation("Invoices");

                    b.Navigation("Rentals");

                    b.Navigation("RoomImages");
                });
#pragma warning restore 612, 618
        }
    }
}

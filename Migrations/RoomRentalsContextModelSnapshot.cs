﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoomRental.Data;

#nullable disable

namespace RoomRental.Migrations
{
    [DbContext(typeof(RoomRentalsContext))]
    partial class RoomRentalsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

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

                    b.Property<byte[]>("FloorPlan")
                        .IsRequired()
                        .HasColumnType("image")
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
                        .HasName("PK__Building__979FD1CD67568AE3");

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

                    b.Property<int?>("RentalOrganizationId")
                        .HasColumnType("int")
                        .HasColumnName("rentalOrganizationId");

                    b.Property<int>("ResponsiblePerson")
                        .HasColumnType("int")
                        .HasColumnName("responsiblePerson");

                    b.Property<int>("RoomId")
                        .HasColumnType("int")
                        .HasColumnName("roomId");

                    b.HasKey("InvoiceId")
                        .HasName("PK__Invoices__1252416CB6BF50FA");

                    b.HasIndex("RentalOrganizationId");

                    b.HasIndex("ResponsiblePerson");

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
                        .HasName("PK__Organiza__29747D592CC83A43");

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
                        .HasName("PK__Rentals__0164732E677AD17B");

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
                        .HasName("PK__Responsi__EC7D7D4DDE04AF9B");

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

                    b.Property<byte[]>("Photo")
                        .IsRequired()
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("photo");

                    b.HasKey("RoomId")
                        .HasName("PK__Rooms__6C3BF5BE42D59D2F");

                    b.HasIndex("BuildingId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("RoomRental.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("RoomRental.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("RoomRental.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RoomRental.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("RoomRental.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoomRental.Models.Building", b =>
                {
                    b.HasOne("RoomRental.Models.Organization", "OwnerOrganization")
                        .WithMany("Buildings")
                        .HasForeignKey("OwnerOrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Buildings__owner__398D8EEE");

                    b.Navigation("OwnerOrganization");
                });

            modelBuilder.Entity("RoomRental.Models.Invoice", b =>
                {
                    b.HasOne("RoomRental.Models.Organization", "RentalOrganization")
                        .WithMany("Invoices")
                        .HasForeignKey("RentalOrganizationId")
                        .HasConstraintName("FK__Invoices__rental__44FF419A");

                    b.HasOne("RoomRental.Models.ResponsiblePerson", "ResponsiblePersonNavigation")
                        .WithMany("Invoices")
                        .HasForeignKey("ResponsiblePerson")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Invoices__respon__46E78A0C");

                    b.HasOne("RoomRental.Models.Room", "Room")
                        .WithMany("Invoices")
                        .HasForeignKey("RoomId")
                        .IsRequired()
                        .HasConstraintName("FK__Invoices__roomId__45F365D3");

                    b.Navigation("RentalOrganization");

                    b.Navigation("ResponsiblePersonNavigation");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("RoomRental.Models.Rental", b =>
                {
                    b.HasOne("RoomRental.Models.Organization", "RentalOrganization")
                        .WithMany("Rentals")
                        .HasForeignKey("RentalOrganizationId")
                        .IsRequired()
                        .HasConstraintName("FK__Rentals__rentalO__403A8C7D");

                    b.HasOne("RoomRental.Models.Room", "Room")
                        .WithMany("Rentals")
                        .HasForeignKey("RoomId")
                        .IsRequired()
                        .HasConstraintName("FK__Rentals__roomId__3F466844");

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
                        .HasConstraintName("FK__Rooms__buildingI__3C69FB99");

                    b.Navigation("Building");
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
                });
#pragma warning restore 612, 618
        }
    }
}

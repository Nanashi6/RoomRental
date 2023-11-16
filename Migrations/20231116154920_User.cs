using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomRental.Migrations
{
    /// <inheritdoc />
    public partial class User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "AspNetRoles",
               columns: table => new
               {
                   Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                   Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                   NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                   ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_AspNetRoles", x => x.Id);
               });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });


            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Buildings__owner__37703C52",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK__Invoices__rental__42E1EEFE",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK__Invoices__respon__44CA3770",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK__Invoices__roomId__43D61337",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK__Rentals__rentalO__3E1D39E1",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK__Rentals__roomId__3D2915A8",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK__RoomImage__roomI__47A6A41B",
                table: "RoomImages");

            migrationBuilder.DropForeignKey(
                name: "FK__Rooms__buildingI__3A4CA8FD",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Rooms__6C3BF5BEA3A3945D",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK__RoomImag__336E9B55CAABD719",
                table: "RoomImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Responsi__EC7D7D4D5E9ADA9C",
                table: "ResponsiblePeople");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Rentals__1D4A79C91609EF5D",
                table: "Rentals");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Organiza__29747D5940E6BB82",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Invoices__DDA6423A7D292366",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Building__979FD1CD3DCCFF84",
                table: "Buildings");

            migrationBuilder.RenameColumn(
                name: "roomId",
                table: "RoomImages",
                newName: "RoomId");

            migrationBuilder.RenameColumn(
                name: "imagePath",
                table: "RoomImages",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "imageId",
                table: "RoomImages",
                newName: "ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomImages_roomId",
                table: "RoomImages",
                newName: "IX_RoomImages_RoomId");

            migrationBuilder.RenameColumn(
                name: "responsiblePersonId",
                table: "Invoices",
                newName: "responsiblePerson");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_responsiblePersonId",
                table: "Invoices",
                newName: "IX_Invoices_responsiblePerson");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "RoomImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "checkOutDate",
                table: "Rentals",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "checkInDate",
                table: "Rentals",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<int>(
                name: "rentalOrganizationId",
                table: "Invoices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "floorPlan",
                table: "Buildings",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Rooms__6C3BF5BE42D59D2F",
                table: "Rooms",
                column: "roomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomImages",
                table: "RoomImages",
                column: "ImageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Responsi__EC7D7D4DDE04AF9B",
                table: "ResponsiblePeople",
                column: "personId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Rentals__0164732E677AD17B",
                table: "Rentals",
                column: "rentalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Organiza__29747D592CC83A43",
                table: "Organizations",
                column: "organizationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Invoices__1252416CB6BF50FA",
                table: "Invoices",
                column: "invoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Building__979FD1CD67568AE3",
                table: "Buildings",
                column: "buildingId");

            migrationBuilder.AddForeignKey(
                name: "FK__Buildings__owner__398D8EEE",
                table: "Buildings",
                column: "ownerOrganizationId",
                principalTable: "Organizations",
                principalColumn: "organizationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Invoices__rental__44FF419A",
                table: "Invoices",
                column: "rentalOrganizationId",
                principalTable: "Organizations",
                principalColumn: "organizationId");

            migrationBuilder.AddForeignKey(
                name: "FK__Invoices__respon__46E78A0C",
                table: "Invoices",
                column: "responsiblePerson",
                principalTable: "ResponsiblePeople",
                principalColumn: "personId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Invoices__roomId__45F365D3",
                table: "Invoices",
                column: "roomId",
                principalTable: "Rooms",
                principalColumn: "roomId");

            migrationBuilder.AddForeignKey(
                name: "FK__Rentals__rentalO__403A8C7D",
                table: "Rentals",
                column: "rentalOrganizationId",
                principalTable: "Organizations",
                principalColumn: "organizationId");

            migrationBuilder.AddForeignKey(
                name: "FK__Rentals__roomId__3F466844",
                table: "Rentals",
                column: "roomId",
                principalTable: "Rooms",
                principalColumn: "roomId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomImages_Rooms_RoomId",
                table: "RoomImages",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "roomId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Rooms__buildingI__3C69FB99",
                table: "Rooms",
                column: "buildingId",
                principalTable: "Buildings",
                principalColumn: "buildingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace PetClinicAppointmentSystem.Model.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvailableAppointmentTime",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    Actived = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    AppointmentTime = table.Column<DateTime>(nullable: false),
                    Time = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableAppointmentTime", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Yetki",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    Actived = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 63, nullable: true),
                    Description = table.Column<string>(maxLength: 127, nullable: true),
                    Showed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yetki", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    Actived = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 63, nullable: true),
                    Surname = table.Column<string>(maxLength: 63, nullable: true),
                    Email = table.Column<string>(maxLength: 127, nullable: true),
                    Password = table.Column<string>(maxLength: 63, nullable: true),
                    TuzlamaDegeri = table.Column<string>(maxLength: 63, nullable: true),
                    KimlikNo = table.Column<string>(maxLength: 31, nullable: true),
                    Resim = table.Column<string>(maxLength: 255, nullable: true),
                    DogumYeri = table.Column<string>(maxLength: 63, nullable: true),
                    DogumTarihi = table.Column<DateTime>(nullable: true),
                    YetkiId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Yetki_YetkiId",
                        column: x => x.YetkiId,
                        principalTable: "Yetki",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Giris",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    Actived = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Token = table.Column<string>(maxLength: 511, nullable: true),
                    Durum = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Giris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Giris_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    Actived = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 63, nullable: true),
                    Resim = table.Column<string>(maxLength: 255, nullable: true),
                    DogumYeri = table.Column<string>(maxLength: 63, nullable: true),
                    DogumTarihi = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pet_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Guid = table.Column<Guid>(nullable: false),
                    Actived = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    AppointmentTime = table.Column<DateTime>(nullable: false),
                    Time = table.Column<int>(nullable: false),
                    PetId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointment_Pet_PetId",
                        column: x => x.PetId,
                        principalTable: "Pet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_PetId",
                table: "Appointment",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_UserId",
                table: "Appointment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Giris_UserId",
                table: "Giris",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pet_UserId",
                table: "Pet",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_YetkiId",
                table: "User",
                column: "YetkiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "AvailableAppointmentTime");

            migrationBuilder.DropTable(
                name: "Giris");

            migrationBuilder.DropTable(
                name: "Pet");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Yetki");
        }
    }
}

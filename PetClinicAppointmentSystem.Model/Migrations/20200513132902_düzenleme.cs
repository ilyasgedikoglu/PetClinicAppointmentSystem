using Microsoft.EntityFrameworkCore.Migrations;

namespace PetClinicAppointmentSystem.Model.Migrations
{
    public partial class düzenleme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Appointment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Time",
                table: "Appointment",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

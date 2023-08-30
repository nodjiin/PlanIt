using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanIt.Persistence.Migrations
{
    public partial class ModifiedGenericId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Plans",
                newName: "PlanId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Availabilities",
                newName: "AvailabilityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PlanId",
                table: "Plans",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AvailabilityId",
                table: "Availabilities",
                newName: "Id");
        }
    }
}

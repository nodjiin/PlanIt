using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanIt.Persistence.Migrations
{
    public partial class availabilities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availability_Users_UserId",
                table: "Availability");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Availability",
                table: "Availability");

            migrationBuilder.RenameTable(
                name: "Availability",
                newName: "Availabilities");

            migrationBuilder.RenameIndex(
                name: "IX_Availability_UserId",
                table: "Availabilities",
                newName: "IX_Availabilities_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Availabilities",
                table: "Availabilities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilities_Users_UserId",
                table: "Availabilities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_Users_UserId",
                table: "Availabilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Availabilities",
                table: "Availabilities");

            migrationBuilder.RenameTable(
                name: "Availabilities",
                newName: "Availability");

            migrationBuilder.RenameIndex(
                name: "IX_Availabilities_UserId",
                table: "Availability",
                newName: "IX_Availability_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Availability",
                table: "Availability",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Availability_Users_UserId",
                table: "Availability",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

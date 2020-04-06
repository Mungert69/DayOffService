using Microsoft.EntityFrameworkCore.Migrations;

namespace DaysOff.Migrations
{
    public partial class AddDishDaysDuratoin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "DishDays",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "DishDays");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PlcApi.Migrations
{
    public partial class Init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Slot",
                table: "Models",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<short>(
                name: "Rack",
                table: "Models",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Slot",
                table: "Models",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "Rack",
                table: "Models",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");
        }
    }
}

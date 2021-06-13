using Microsoft.EntityFrameworkCore.Migrations;

namespace PlcApi.Migrations
{
    public partial class sensorsandpallets2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pallets_Conveyors_ConveyorId",
                table: "Pallets");

            migrationBuilder.AlterColumn<int>(
                name: "ConveyorId",
                table: "Pallets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Pallets_Conveyors_ConveyorId",
                table: "Pallets",
                column: "ConveyorId",
                principalTable: "Conveyors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pallets_Conveyors_ConveyorId",
                table: "Pallets");

            migrationBuilder.AlterColumn<int>(
                name: "ConveyorId",
                table: "Pallets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pallets_Conveyors_ConveyorId",
                table: "Pallets",
                column: "ConveyorId",
                principalTable: "Conveyors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PlcApi.Migrations
{
    public partial class conveyors1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_Conveyors_ConveyorId",
                table: "Blocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Blocks",
                table: "Blocks");

            migrationBuilder.RenameTable(
                name: "Blocks",
                newName: "Pallets");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Conveyors",
                newName: "ConveyorId");

            migrationBuilder.RenameIndex(
                name: "IX_Blocks_ConveyorId",
                table: "Pallets",
                newName: "IX_Pallets_ConveyorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pallets",
                table: "Pallets",
                column: "PalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pallets_Conveyors_ConveyorId",
                table: "Pallets",
                column: "ConveyorId",
                principalTable: "Conveyors",
                principalColumn: "ConveyorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pallets_Conveyors_ConveyorId",
                table: "Pallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pallets",
                table: "Pallets");

            migrationBuilder.RenameTable(
                name: "Pallets",
                newName: "Blocks");

            migrationBuilder.RenameColumn(
                name: "ConveyorId",
                table: "Conveyors",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Pallets_ConveyorId",
                table: "Blocks",
                newName: "IX_Blocks_ConveyorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Blocks",
                table: "Blocks",
                column: "PalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blocks_Conveyors_ConveyorId",
                table: "Blocks",
                column: "ConveyorId",
                principalTable: "Conveyors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PlcApi.Migrations
{
    public partial class conveyors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_Conveyors_ConveyorId",
                table: "Blocks");

            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_PLCs_PlcId",
                table: "Blocks");

            migrationBuilder.DropIndex(
                name: "IX_Blocks_PlcId",
                table: "Blocks");

            migrationBuilder.RenameColumn(
                name: "ConveyorId",
                table: "Conveyors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PlcId",
                table: "Blocks",
                newName: "BoardId");

            migrationBuilder.RenameColumn(
                name: "BlockId",
                table: "Blocks",
                newName: "PalletId");

            migrationBuilder.AlterColumn<int>(
                name: "ConveyorId",
                table: "Blocks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Blocks_Conveyors_ConveyorId",
                table: "Blocks",
                column: "ConveyorId",
                principalTable: "Conveyors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_Conveyors_ConveyorId",
                table: "Blocks");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Conveyors",
                newName: "ConveyorId");

            migrationBuilder.RenameColumn(
                name: "BoardId",
                table: "Blocks",
                newName: "PlcId");

            migrationBuilder.RenameColumn(
                name: "PalletId",
                table: "Blocks",
                newName: "BlockId");

            migrationBuilder.AlterColumn<int>(
                name: "ConveyorId",
                table: "Blocks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_PlcId",
                table: "Blocks",
                column: "PlcId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blocks_Conveyors_ConveyorId",
                table: "Blocks",
                column: "ConveyorId",
                principalTable: "Conveyors",
                principalColumn: "ConveyorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blocks_PLCs_PlcId",
                table: "Blocks",
                column: "PlcId",
                principalTable: "PLCs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PlcApi.Migrations
{
    public partial class ConveyorUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "Conveyors");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "Conveyors");

            migrationBuilder.AddColumn<bool>(
                name: "IsRunning",
                table: "Conveyors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ConveyorId",
                table: "Blocks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConveyorPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConveyorId = table.Column<int>(type: "int", nullable: false),
                    isMainPoint = table.Column<bool>(type: "bit", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConveyorPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConveyorPoints_Conveyors_ConveyorId",
                        column: x => x.ConveyorId,
                        principalTable: "Conveyors",
                        principalColumn: "ConveyorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_ConveyorId",
                table: "Blocks",
                column: "ConveyorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConveyorPoints_ConveyorId",
                table: "ConveyorPoints",
                column: "ConveyorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blocks_Conveyors_ConveyorId",
                table: "Blocks",
                column: "ConveyorId",
                principalTable: "Conveyors",
                principalColumn: "ConveyorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_Conveyors_ConveyorId",
                table: "Blocks");

            migrationBuilder.DropTable(
                name: "ConveyorPoints");

            migrationBuilder.DropIndex(
                name: "IX_Blocks_ConveyorId",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "IsRunning",
                table: "Conveyors");

            migrationBuilder.DropColumn(
                name: "ConveyorId",
                table: "Blocks");

            migrationBuilder.AddColumn<int>(
                name: "X",
                table: "Conveyors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Y",
                table: "Conveyors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

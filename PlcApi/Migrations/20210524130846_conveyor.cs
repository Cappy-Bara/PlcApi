using Microsoft.EntityFrameworkCore.Migrations;

namespace PlcApi.Migrations
{
    public partial class conveyor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conveyors",
                columns: table => new
                {
                    ConveyorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    IsVertical = table.Column<bool>(type: "bit", nullable: false),
                    IsTurnedDownOrLeft = table.Column<bool>(type: "bit", nullable: false),
                    Length = table.Column<int>(type: "int", nullable: false),
                    Speed = table.Column<int>(type: "int", nullable: false),
                    InputOutputId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conveyors", x => x.ConveyorId);
                    table.ForeignKey(
                        name: "FK_Conveyors_InputsOutputs_InputOutputId",
                        column: x => x.InputOutputId,
                        principalTable: "InputsOutputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conveyors_InputOutputId",
                table: "Conveyors",
                column: "InputOutputId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conveyors");
        }
    }
}

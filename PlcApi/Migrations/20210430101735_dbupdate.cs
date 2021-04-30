using Microsoft.EntityFrameworkCore.Migrations;

namespace PlcApi.Migrations
{
    public partial class dbupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CpuModel = table.Column<int>(type: "int", nullable: false),
                    Cpu = table.Column<int>(type: "int", nullable: false),
                    Rack = table.Column<short>(type: "smallint", nullable: false),
                    Slot = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PLCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLCs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PLCs_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InputsOutputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bit = table.Column<int>(type: "int", nullable: false),
                    Byte = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PlcId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputsOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InputsOutputs_PLCs_PlcId",
                        column: x => x.PlcId,
                        principalTable: "PLCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InputsOutputs_PlcId",
                table: "InputsOutputs",
                column: "PlcId");

            migrationBuilder.CreateIndex(
                name: "IX_PLCs_ModelId",
                table: "PLCs",
                column: "ModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InputsOutputs");

            migrationBuilder.DropTable(
                name: "PLCs");

            migrationBuilder.DropTable(
                name: "Models");
        }
    }
}

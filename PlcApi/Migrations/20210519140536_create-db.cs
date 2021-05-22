﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace PlcApi.Migrations
{
    public partial class createdb : Migration
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
                name: "Blocks",
                columns: table => new
                {
                    BlockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PosX = table.Column<int>(type: "int", nullable: false),
                    PosY = table.Column<int>(type: "int", nullable: false),
                    PlcId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.BlockId);
                    table.ForeignKey(
                        name: "FK_Blocks_PLCs_PlcId",
                        column: x => x.PlcId,
                        principalTable: "PLCs",
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

            migrationBuilder.CreateTable(
                name: "Diodes",
                columns: table => new
                {
                    DiodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosX = table.Column<int>(type: "int", nullable: false),
                    PosY = table.Column<int>(type: "int", nullable: false),
                    InputOutputId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diodes", x => x.DiodeId);
                    table.ForeignKey(
                        name: "FK_Diodes_InputsOutputs_InputOutputId",
                        column: x => x.InputOutputId,
                        principalTable: "InputsOutputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_PlcId",
                table: "Blocks",
                column: "PlcId");

            migrationBuilder.CreateIndex(
                name: "IX_Diodes_InputOutputId",
                table: "Diodes",
                column: "InputOutputId");

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
                name: "Blocks");

            migrationBuilder.DropTable(
                name: "Diodes");

            migrationBuilder.DropTable(
                name: "InputsOutputs");

            migrationBuilder.DropTable(
                name: "PLCs");

            migrationBuilder.DropTable(
                name: "Models");
        }
    }
}
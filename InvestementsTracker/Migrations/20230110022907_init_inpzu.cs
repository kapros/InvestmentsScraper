using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InvestementsTracker.Migrations
{
    /// <inheritdoc />
    public partial class initinpzu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InPzuPortfolios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Since = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InPzuPortfolios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InPzuOrders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderName = table.Column<string>(type: "text", nullable: false),
                    PurchasedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PurchaseWorth = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    PortfolioId = table.Column<long>(type: "bigint", nullable: false),
                    OrderType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InPzuOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InPzuOrders_InPzuPortfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "InPzuPortfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InPzuPositions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Units = table.Column<double>(type: "double precision", nullable: false),
                    PriceOfUnit = table.Column<double>(type: "double precision", nullable: false),
                    PurchaseValue = table.Column<double>(type: "double precision", nullable: false),
                    FundId = table.Column<string>(type: "text", nullable: false),
                    RegistrationId = table.Column<string>(type: "text", nullable: false),
                    FundName = table.Column<string>(type: "text", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UnitType = table.Column<string>(type: "text", nullable: false),
                    PortfolioId = table.Column<long>(type: "bigint", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    InPzuOrderId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InPzuPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InPzuPositions_InPzuOrders_InPzuOrderId",
                        column: x => x.InPzuOrderId,
                        principalTable: "InPzuOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InPzuPositions_InPzuPortfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "InPzuPortfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InPzuResults",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PositionId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    Percentile = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InPzuResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InPzuResults_InPzuPositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "InPzuPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InPzuOrders_PortfolioId",
                table: "InPzuOrders",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_InPzuPositions_InPzuOrderId",
                table: "InPzuPositions",
                column: "InPzuOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_InPzuPositions_PortfolioId",
                table: "InPzuPositions",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_InPzuResults_PositionId",
                table: "InPzuResults",
                column: "PositionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InPzuResults");

            migrationBuilder.DropTable(
                name: "InPzuPositions");

            migrationBuilder.DropTable(
                name: "InPzuOrders");

            migrationBuilder.DropTable(
                name: "InPzuPortfolios");
        }
    }
}

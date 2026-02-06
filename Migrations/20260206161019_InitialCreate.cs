using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InterviewTask.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllergyCheckLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Query = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DrugKey = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    AllergensRaw = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ResultJson = table.Column<string>(type: "text", nullable: false),
                    CheckedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllergyCheckLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AvailabilityRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DrugKey = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailabilityRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DrugKey = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItem", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllergyCheckLog_CheckedAtUtc",
                table: "AllergyCheckLog",
                column: "CheckedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_AllergyCheckLog_DrugKey",
                table: "AllergyCheckLog",
                column: "DrugKey");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityRequest_Email_DrugKey",
                table: "AvailabilityRequest",
                columns: new[] { "Email", "DrugKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityRequest_Status",
                table: "AvailabilityRequest",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_DrugKey",
                table: "InventoryItem",
                column: "DrugKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllergyCheckLog");

            migrationBuilder.DropTable(
                name: "AvailabilityRequest");

            migrationBuilder.DropTable(
                name: "InventoryItem");
        }
    }
}

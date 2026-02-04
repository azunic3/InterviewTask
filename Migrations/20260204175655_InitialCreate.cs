using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugKey = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    AllergensRaw = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ResultJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllergyCheckLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AvailabilityRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugKey = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailabilityRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrugLabelCache",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SetId = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    QueryKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GenericName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ManufacturerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    JsonLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CachedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugLabelCache", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugKey = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "IX_AvailabilityRequest_DrugKey_Status",
                table: "AvailabilityRequest",
                columns: new[] { "DrugKey", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityRequest_Email",
                table: "AvailabilityRequest",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_DrugLabelCache_QueryKey",
                table: "DrugLabelCache",
                column: "QueryKey");

            migrationBuilder.CreateIndex(
                name: "IX_DrugLabelCache_SetId",
                table: "DrugLabelCache",
                column: "SetId",
                unique: true);

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
                name: "DrugLabelCache");

            migrationBuilder.DropTable(
                name: "InventoryItem");
        }
    }
}

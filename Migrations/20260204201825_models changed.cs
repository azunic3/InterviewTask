using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewTask.Migrations
{
    /// <inheritdoc />
    public partial class modelschanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AvailabilityRequest_DrugKey_Status",
                table: "AvailabilityRequest");

            migrationBuilder.DropIndex(
                name: "IX_AvailabilityRequest_Email",
                table: "AvailabilityRequest");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "InventoryItem");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "InventoryItem",
                newName: "LastUpdated");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "InventoryItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Query",
                table: "AllergyCheckLog",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityRequest_Email_DrugKey",
                table: "AvailabilityRequest",
                columns: new[] { "Email", "DrugKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityRequest_Status",
                table: "AvailabilityRequest",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AvailabilityRequest_Email_DrugKey",
                table: "AvailabilityRequest");

            migrationBuilder.DropIndex(
                name: "IX_AvailabilityRequest_Status",
                table: "AvailabilityRequest");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "Query",
                table: "AllergyCheckLog");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "InventoryItem",
                newName: "UpdatedAtUtc");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "InventoryItem",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityRequest_DrugKey_Status",
                table: "AvailabilityRequest",
                columns: new[] { "DrugKey", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityRequest_Email",
                table: "AvailabilityRequest",
                column: "Email");
        }
    }
}

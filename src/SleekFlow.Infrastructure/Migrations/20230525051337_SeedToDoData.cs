using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SleekFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedToDoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var addAtDate = new DateTime(2023, 5, 25, 0, 0, 0, DateTimeKind.Utc);
            var firstDueDate = new DateTime(2023, 5, 27, 0, 0, 0, DateTimeKind.Utc);
            var secondDueDate = new DateTime(2023, 5, 28, 0, 0, 0, DateTimeKind.Utc);

            migrationBuilder.InsertData(
                table: "ToDos",
                columns: new[] { "Id", "Name", "Description", "DueAt", "Status", "AddAt", "AddBy", "EditAt", "EditBy" },
                values: new object[] { 1, "Clean", "Place clothes in washing machine", firstDueDate, 1, addAtDate, "System", null, null });

            migrationBuilder.InsertData(
                table: "ToDos",
                columns: new[] { "Id", "Name", "Description", "DueAt", "Status", "AddAt", "AddBy", "EditAt", "EditBy" },
                values: new object[] { 2, "Dry", "Hang clothes to dry", secondDueDate, 0, addAtDate, "System", null, null });

            migrationBuilder.InsertData(
                table: "ToDos",
                columns: new[] { "Id", "Name", "Description", "DueAt", "Status", "AddAt", "AddBy", "EditAt", "EditBy" },
                values: new object[] { 3, "Iron", "Iron clothes", secondDueDate, 0, addAtDate, "System", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                    table: "ToDos",
                    keyColumn: "Id",
                    keyValues: new object[] { 1, 2, 3 });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "3eb7c48d-7511-4712-a4ee-c3d2b1f41fb2");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "80f9ec2f-869e-468b-afa3-56e0a7d79ef9");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "8c8ab4b0-f1c5-47b0-bf32-d7054251e48b");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "cd1a3aee-fb30-4cc2-9cde-ddbd7012461a");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e1834fb9-24c7-4960-8244-1c57d14f55c0");

            migrationBuilder.RenameColumn(
                name: "Id_new",
                table: "Claims",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Claims",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "Permission",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "Type" },
                values: new object[] { "role", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Type" },
                values: new object[] { "role", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Type" },
                values: new object[] { "role", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Type" },
                values: new object[] { "role", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Type" },
                values: new object[] { "role", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Type" },
                values: new object[] { "claim", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Type" },
                values: new object[] { "claim", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Type" },
                values: new object[] { "claim", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "Type" },
                values: new object[] { "claim", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Category", "Type" },
                values: new object[] { "category", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Category", "Type" },
                values: new object[] { "category", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Category", "Type" },
                values: new object[] { "category", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Category", "Type" },
                values: new object[] { "class", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Category", "Type" },
                values: new object[] { "class", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Category", "Type" },
                values: new object[] { "class", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Category", "Type" },
                values: new object[] { "class", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Category", "Type" },
                values: new object[] { "class", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Category", "Type" },
                values: new object[] { "course", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Category", "Type" },
                values: new object[] { "course", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Category", "Type" },
                values: new object[] { "course", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Category", "Type" },
                values: new object[] { "enrollment", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Category", "Type" },
                values: new object[] { "enrollment", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Category", "Type" },
                values: new object[] { "lesson", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Category", "Type" },
                values: new object[] { "lesson", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Category", "Type" },
                values: new object[] { "lesson", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Category", "Type" },
                values: new object[] { "order", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Category", "Type" },
                values: new object[] { "order", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Category", "Type" },
                values: new object[] { "student", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Category", "Type" },
                values: new object[] { "student", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Category", "Type" },
                values: new object[] { "student", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Category", "Type" },
                values: new object[] { "teacher", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Category", "Type" },
                values: new object[] { "teacher", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Category", "Type" },
                values: new object[] { "teacher", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Category", "Type" },
                values: new object[] { "attendance", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Category", "Type" },
                values: new object[] { "attendance", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Category", "Type" },
                values: new object[] { "attendance", "Permission" });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "Category", "Type" },
                values: new object[] { "account", "Permission" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0db487f0-34b6-41d6-8d57-79c68e29bc49", null, "Teacher", "TEACHER" },
                    { "1cc1798d-5ae6-49a9-a13c-674d460be668", null, "Student", "STUDENT" },
                    { "4786976d-1216-4038-8333-15edc74f9825", null, "Admin", "ADMIN" },
                    { "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8", null, "SuperAdmin", "SUPERADMIN" },
                    { "fb644210-aa19-4119-96af-d1ba6e1bdecf", null, "Employee", "EMPLOYEE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "0db487f0-34b6-41d6-8d57-79c68e29bc49");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "1cc1798d-5ae6-49a9-a13c-674d460be668");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4786976d-1216-4038-8333-15edc74f9825");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "fb644210-aa19-4119-96af-d1ba6e1bdecf");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Claims");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Claims",
                newName: "Id_new");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Claims",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldDefaultValue: "Permission");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 1,
                column: "Type",
                value: "role");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 2,
                column: "Type",
                value: "role");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 3,
                column: "Type",
                value: "role");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 4,
                column: "Type",
                value: "role");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 5,
                column: "Type",
                value: "role");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 6,
                column: "Type",
                value: "claim");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 7,
                column: "Type",
                value: "claim");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 8,
                column: "Type",
                value: "claim");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 9,
                column: "Type",
                value: "claim");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 10,
                column: "Type",
                value: "category");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 11,
                column: "Type",
                value: "category");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 12,
                column: "Type",
                value: "category");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 13,
                column: "Type",
                value: "class");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 14,
                column: "Type",
                value: "class");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 15,
                column: "Type",
                value: "class");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 16,
                column: "Type",
                value: "class");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 17,
                column: "Type",
                value: "class");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 18,
                column: "Type",
                value: "course");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 19,
                column: "Type",
                value: "course");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 20,
                column: "Type",
                value: "course");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 21,
                column: "Type",
                value: "enrollment");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 22,
                column: "Type",
                value: "enrollment");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 23,
                column: "Type",
                value: "lesson");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 24,
                column: "Type",
                value: "lesson");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 25,
                column: "Type",
                value: "lesson");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 26,
                column: "Type",
                value: "order");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 27,
                column: "Type",
                value: "order");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 28,
                column: "Type",
                value: "student");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 29,
                column: "Type",
                value: "student");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 30,
                column: "Type",
                value: "student");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 31,
                column: "Type",
                value: "teacher");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 32,
                column: "Type",
                value: "teacher");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 33,
                column: "Type",
                value: "teacher");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 34,
                column: "Type",
                value: "attendance");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 35,
                column: "Type",
                value: "attendance");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 36,
                column: "Type",
                value: "attendance");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id_new",
                keyValue: 37,
                column: "Type",
                value: "account");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3eb7c48d-7511-4712-a4ee-c3d2b1f41fb2", null, "Student", "STUDENT" },
                    { "80f9ec2f-869e-468b-afa3-56e0a7d79ef9", null, "Admin", "ADMIN" },
                    { "8c8ab4b0-f1c5-47b0-bf32-d7054251e48b", null, "SuperAdmin", "SUPERADMIN" },
                    { "cd1a3aee-fb30-4cc2-9cde-ddbd7012461a", null, "Employee", "EMPLOYEE" },
                    { "e1834fb9-24c7-4960-8244-1c57d14f55c0", null, "Teacher", "TEACHER" }
                });
        }
    }
}

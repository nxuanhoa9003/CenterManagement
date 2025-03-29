using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2a227b05-7a70-4d7b-bf32-25fc8f7385a1", null, "Student", "STUDENT" },
                    { "890df94c-044a-4683-ab11-8b18a4530bda", null, "Employee", "EMPLOYEE" },
                    { "8c82c6d7-6f11-4b9c-a6d6-66221e68632f", null, "Teacher", "TEACHER" },
                    { "cd84f029-1135-442c-a68e-e8d0fcef9eba", null, "Admin", "ADMIN" },
                    { "d633bea3-aad8-4e31-bc14-e029f78a3fdc", null, "SuperAdmin", "SUPERADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2a227b05-7a70-4d7b-bf32-25fc8f7385a1");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "890df94c-044a-4683-ab11-8b18a4530bda");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "8c82c6d7-6f11-4b9c-a6d6-66221e68632f");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "cd84f029-1135-442c-a68e-e8d0fcef9eba");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "d633bea3-aad8-4e31-bc14-e029f78a3fdc");
        }
    }
}

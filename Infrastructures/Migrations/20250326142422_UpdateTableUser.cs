using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true,
                oldCollation: "SQL_Latin1_General_CP1_CS_AS");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "030c7070-b36a-4299-a645-a985bf2be7f6", null, "Student", "STUDENT" },
                    { "42a17c66-f7a3-407b-bbd1-6e975128fea3", null, "Teacher", "TEACHER" },
                    { "936c2e9f-6f40-4d13-bce8-f1fc9d45f787", null, "SuperAdmin", "SUPERADMIN" },
                    { "bb6957e5-71a5-4db5-b598-d8666c5fe170", null, "Employee", "EMPLOYEE" },
                    { "e0b0c38e-6773-4d14-99f2-989799568b91", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "030c7070-b36a-4299-a645-a985bf2be7f6");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "42a17c66-f7a3-407b-bbd1-6e975128fea3");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "936c2e9f-6f40-4d13-bce8-f1fc9d45f787");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "bb6957e5-71a5-4db5-b598-d8666c5fe170");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e0b0c38e-6773-4d14-99f2-989799568b91");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                collation: "SQL_Latin1_General_CP1_CS_AS",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

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
    }
}

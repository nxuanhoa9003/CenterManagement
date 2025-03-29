using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableClaims2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 36);

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
                keyValue: "fb644210-aa19-4119-96af-d1ba6e1bdecf");

            migrationBuilder.InsertData(
                table: "RoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permission", "CanCreateRole", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 2, "Permission", "CanUpdateRole", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 3, "Permission", "CanDeleteRole", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 4, "Permission", "CanAssignRoleToUser", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 5, "Permission", "CanRemoveRoleFromUser", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 6, "Permission", "CanAddClaimToUser", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 7, "Permission", "CanRemoveClaimFromUser", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 8, "Permission", "CanAddClaimToRole", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 9, "Permission", "CanRemoveClaimFromRole", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 10, "Permission", "CanAddCategory", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 11, "Permission", "CanUpdateCategory", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 12, "Permission", "CanDeleteCategory", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 13, "Permission", "CanGetStudentClass", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 14, "Permission", "CanAddClass", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 15, "Permission", "CanUpdateClass", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 16, "Permission", "CanDeleteClass", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 17, "Permission", "CanDeleteStudentFromClass", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 18, "Permission", "CanAddCourse", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 19, "Permission", "CanUpdateCourse", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 20, "Permission", "CanDeleteCourse", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 21, "Permission", "CanEnrollStudent", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 22, "Permission", "CanUnenrollStudent", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 23, "Permission", "CanAddLesson", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 24, "Permission", "CanUpdateLesson", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 25, "Permission", "CanDeleteLesson", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 26, "Permission", "CanGetAllOrders", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 27, "Permission", "CanGetOrderStudent", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 28, "Permission", "CanAddStudent", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 29, "Permission", "CanUpdateStudent", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 30, "Permission", "CanDeleteStudent", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 31, "Permission", "CanAddTeacher", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 32, "Permission", "CanUpdateTeacher", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 33, "Permission", "CanDeleteTeacher", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 34, "Permission", "CanRecordAttendance", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 35, "Permission", "CanUpdateAttendance", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 36, "Permission", "CanDeleteAttendance", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" },
                    { 37, "Permission", "CanCreateAccount", "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a5e8d7f4-5c1e-48c5-b2a1-65b2a7f0f5c8", null, "Employee", "EMPLOYEE" },
                    { "b6d9e8a7-4f2d-4e8a-8b3b-29d7a8c2f8d3", null, "Student", "STUDENT" },
                    { "c3f0a2b9-5c7e-44c9-ae17-784bc4fbb27b", null, "Admin", "ADMIN" },
                    { "e9f2b7c1-8d3a-49c5-b7e1-45a2d8e5c6a9", null, "Teacher", "TEACHER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "a5e8d7f4-5c1e-48c5-b2a1-65b2a7f0f5c8");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "b6d9e8a7-4f2d-4e8a-8b3b-29d7a8c2f8d3");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "c3f0a2b9-5c7e-44c9-ae17-784bc4fbb27b");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "e9f2b7c1-8d3a-49c5-b7e1-45a2d8e5c6a9");

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "Id", "Category", "Name", "Type", "Value" },
                values: new object[] { 36, "attendance", "Quyền xóa điểm danh", "Permission", "CanDeleteAttendance" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0db487f0-34b6-41d6-8d57-79c68e29bc49", null, "Teacher", "TEACHER" },
                    { "1cc1798d-5ae6-49a9-a13c-674d460be668", null, "Student", "STUDENT" },
                    { "4786976d-1216-4038-8333-15edc74f9825", null, "Admin", "ADMIN" },
                    { "fb644210-aa19-4119-96af-d1ba6e1bdecf", null, "Employee", "EMPLOYEE" }
                });
        }
    }
}

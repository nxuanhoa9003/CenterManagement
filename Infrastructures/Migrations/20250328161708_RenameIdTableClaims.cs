using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class RenameIdTableClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // Tạo lại bảng Claims với Id kiểu int tự tăng
            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                });

            migrationBuilder.DropPrimaryKey(
                name: "PK_Claims",
                table: "Claims");

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

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Claims");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Claims",
                type: "nvarchar(100)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Claims",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Id_new",
                table: "Claims",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Claims",
                table: "Claims",
                column: "Id_new");

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "Id_new", "Name", "Type", "Value" },
                values: new object[,]
                {
                    { 1, "Quyền tạo vai trò", "role", "CanCreateRole" },
                    { 2, "Quyền cập nhật vai trò", "role", "CanUpdateRole" },
                    { 3, "Quyền xóa vai trò", "role", "CanDeleteRole" },
                    { 4, "Quyền gán vai trò cho người dùng", "role", "CanAssignRoleToUser" },
                    { 5, "Quyền xóa vai trò khỏi người dùng", "role", "CanRemoveRoleFromUser" },
                    { 6, "Quyền thêm claim cho người dùng", "claim", "CanAddClaimToUser" },
                    { 7, "Quyền xóa claim khỏi người dùng", "claim", "CanRemoveClaimFromUser" },
                    { 8, "Quyền thêm claim cho vai trò", "claim", "CanAddClaimToRole" },
                    { 9, "Quyền xóa claim khỏi vai trò", "claim", "CanRemoveClaimFromRole" },
                    { 10, "Quyền thêm danh mục", "category", "CanAddCategory" },
                    { 11, "Quyền cập nhật danh mục", "category", "CanUpdateCategory" },
                    { 12, "Quyền xóa danh mục", "category", "CanDeleteCategory" },
                    { 13, "Quyền xem danh sách học viên trong lớp", "class", "CanGetStudentClass" },
                    { 14, "Quyền thêm lớp học", "class", "CanAddClass" },
                    { 15, "Quyền cập nhật lớp học", "class", "CanUpdateClass" },
                    { 16, "Quyền xóa lớp học", "class", "CanDeleteClass" },
                    { 17, "Quyền xóa học viên khỏi lớp", "class", "CanDeleteStudentFromClass" },
                    { 18, "Quyền thêm khóa học", "course", "CanAddCourse" },
                    { 19, "Quyền cập nhật khóa học", "course", "CanUpdateCourse" },
                    { 20, "Quyền xóa khóa học", "course", "CanDeleteCourse" },
                    { 21, "Quyền ghi danh học viên", "enrollment", "CanEnrollStudent" },
                    { 22, "Quyền hủy ghi danh học viên", "enrollment", "CanUnenrollStudent" },
                    { 23, "Quyền thêm bài học", "lesson", "CanAddLesson" },
                    { 24, "Quyền cập nhật bài học", "lesson", "CanUpdateLesson" },
                    { 25, "Quyền xóa bài học", "lesson", "CanDeleteLesson" },
                    { 26, "Quyền xem tất cả đơn hàng", "order", "CanGetAllOrders" },
                    { 27, "Quyền xem đơn hàng của học viên", "order", "CanGetOrderStudent" },
                    { 28, "Quyền thêm học viên", "student", "CanAddStudent" },
                    { 29, "Quyền cập nhật học viên", "student", "CanUpdateStudent" },
                    { 30, "Quyền xóa học viên", "student", "CanDeleteStudent" },
                    { 31, "Quyền thêm giáo viên", "teacher", "CanAddTeacher" },
                    { 32, "Quyền cập nhật giáo viên", "teacher", "CanUpdateTeacher" },
                    { 33, "Quyền xóa giáo viên", "teacher", "CanDeleteTeacher" },
                    { 34, "Quyền ghi nhận điểm danh", "attendance", "CanRecordAttendance" },
                    { 35, "Quyền cập nhật điểm danh", "attendance", "CanUpdateAttendance" },
                    { 36, "Quyền xóa điểm danh", "attendance", "CanDeleteAttendance" },
                    { 37, "Quyền tạo tài khoản", "account", "CanCreateAccount" }
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Nếu rollback, xóa bảng Claims
            migrationBuilder.DropTable(name: "Claims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Claims",
                table: "Claims");

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id_new",
                keyColumnType: "int",
                keyValue: 37);

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

            migrationBuilder.DropColumn(
                name: "Id_new",
                table: "Claims");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Claims",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Claims",
                table: "Claims",
                column: "Id");

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
    }
}

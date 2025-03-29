using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain.Enums;
using System.Security.Claims;

namespace Infrastructures
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ClaimEntity> Claims { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshTokens");
                entity.Property(rt => rt.Id)
                      .ValueGeneratedOnAdd();

                entity.HasKey(rt => rt.Id);

                entity.Property(rt => rt.Token)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(rt => rt.UserId)
                    .IsRequired();

                entity.Property(rt => rt.ExpiryDate)
                    .IsRequired();

                entity.Property(rt => rt.IsRevoked)
                    .HasDefaultValue(false);

                entity.Property(rt => rt.IsUsed)
                    .HasDefaultValue(false);

                entity.Property(rt => rt.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()"); // Mặc định là ngày tạo hiện tại

                // Thiết lập quan hệ với bảng User (AspNetUsers)
                entity.HasOne(rt => rt.User)
                    .WithMany() // Không cần danh sách RefreshTokens trong User
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Xóa user thì xóa luôn RefreshToken
            });


            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Classes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnType("nvarchar(100)").IsRequired();

                entity.Property(e => e.StartDate)
                .HasColumnType("date").IsRequired();

                entity.Property(e => e.EndDate)
                .HasColumnType("date").IsRequired();

                // ** Teacher - Class (1 - N) **
                entity.HasOne(e => e.Teacher)
                .WithMany(t => t.Classes)
                .HasForeignKey(e => e.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);

                // ** Course - Class (1 - N) **
                entity.HasOne(e => e.Course)
                .WithMany(course => course.Classes)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.NoAction);
            });



            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.CourseName).HasColumnType("nvarchar(250)").IsRequired();
                entity.Property(e => e.Description).HasColumnType("nvarchar(MAX)").IsRequired();
                entity.Property(e => e.CreatedDate)
                 .HasColumnType("date").IsRequired();

                // ** Category - Course (1 - N) **
                entity.HasOne(e => e.Category)
                .WithMany(cat => cat.Courses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
            });


            // ** Class - Lesson (1 - N) **
            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("Lessons");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasColumnType("nvarchar(100)").IsRequired();
                entity.Property(e => e.Description).HasColumnType("nvarchar(Max)").IsRequired();

                // ** Class - Lesson (1 - N) **
                entity.HasOne(l => l.Class)
                .WithMany(e => e.Lessons)
                .HasForeignKey(l => l.ClassId)
                .OnDelete(DeleteBehavior.SetNull);
            });



            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("Attendances");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Date)
               .HasColumnType("date").IsRequired();

                // ** Lesson - Attendance (1 - N) **
                entity.HasOne(a => a.Lesson)
                .WithMany(l => l.Attendances)
                .HasForeignKey(a => a.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

                // ** Student - Attendance (1 - N) **
                entity.HasOne(a => a.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.ToTable("Enrollments");

                // ** Student - Enrollment - Class (N - N) **
                entity.HasKey(e => new { e.StudentId, e.ClassId });

                entity.HasIndex(e => new { e.StudentId, e.ClassId }).IsUnique();

                entity.HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);


                entity.HasOne(e => e.Class)
                .WithMany(e => e.Enrollments)
                .HasForeignKey(e => e.ClassId)
                .OnDelete(DeleteBehavior.Cascade);
            });



            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payments");
                entity.HasKey(e => e.Id);

                // Payment - Order (One-to-One)
                entity.HasOne(p => p.Order)
                    .WithOne(o => o.Payment)
                    .HasForeignKey<Payment>(p => p.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Student)
                    .WithMany(s => s.Payments)
                    .HasForeignKey(p => p.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Order>(entity =>
            {
                // Order - Student (Many-to-One)
                entity.HasOne(o => o.Student)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

                // Order - Course (Many-to-One)
                entity.HasOne(o => o.Course)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ClaimEntity>(entity =>
            {
                entity.ToTable("Claims");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd(); // Đảm bảo ID tự tăng

                entity.Property(e => e.Type).HasColumnType("nvarchar(50)")
                    .HasDefaultValue("Permission")
                    .IsRequired();

                entity.Property(e => e.Value)
                      .HasColumnType("nvarchar(100)")
                      .IsRequired()
                      .IsUnicode(false); // Nếu không cần ký tự Unicode, giúp tối ưu DB

                entity.Property(e => e.Name)
                      .HasColumnType("nvarchar(100)")
                      .IsRequired();


            });


            // Liên kết ApplicationUser với Student
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");
                entity.HasKey(e => e.StudentId);

                entity.Property(e => e.CreatedAt)
                .HasColumnType("date").IsRequired();

                entity.HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });


            // Liên kết ApplicationUser với Teacher
            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teachers");
                entity.HasKey(e => e.TeacherId);
                entity.Property(e => e.CreatedAt)
                .HasColumnType("date").IsRequired();
                entity.HasOne(t => t.User)
                .WithOne(u => u.Teacher)
                .HasForeignKey<Teacher>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ApplicationUser>(e =>
            {
                e.Property(x => x.FullName).HasColumnType("nvarchar(100)").IsRequired();
                e.HasIndex(u => u.NormalizedUserName).IsUnique();
            });


            //modelBuilder.Entity<ApplicationUser>()
            //   .Property(u => u.UserName)
            //   .UseCollation("SQL_Latin1_General_CP1_CS_AS");


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName != null && tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            // update data table roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole { Id = "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8", Name = RoleUser.SuperAdmin, NormalizedName = RoleUser.SuperAdmin.ToUpper() },
                new IdentityRole { Id = "c3f0a2b9-5c7e-44c9-ae17-784bc4fbb27b", Name = RoleUser.Admin, NormalizedName = RoleUser.Admin.ToUpper() },
                new IdentityRole { Id = "a5e8d7f4-5c1e-48c5-b2a1-65b2a7f0f5c8", Name = RoleUser.Employee, NormalizedName = RoleUser.Employee.ToUpper() },
                new IdentityRole { Id = "b6d9e8a7-4f2d-4e8a-8b3b-29d7a8c2f8d3", Name = RoleUser.Student, NormalizedName = RoleUser.Student.ToUpper() },
                new IdentityRole { Id = "e9f2b7c1-8d3a-49c5-b7e1-45a2d8e5c6a9", Name = RoleUser.Teacher, NormalizedName = RoleUser.Teacher.ToUpper() }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);


            // add data table claim
            AddDataClaims(modelBuilder);

        }

        private void AddDataClaims(ModelBuilder modelBuilder)
        {
            var claims = new List<ClaimEntity>
            {
                // Role Management
                new() { Id = 1, Type = "Permission", Category = "role", Value = Permission.CanCreateRole.ToString(), Name = "Quyền tạo vai trò" },
                new() { Id = 2, Type = "Permission", Category = "role", Value = Permission.CanUpdateRole.ToString(), Name = "Quyền cập nhật vai trò" },
                new() { Id = 3, Type = "Permission", Category = "role", Value = Permission.CanDeleteRole.ToString(), Name = "Quyền xóa vai trò" },
                new() { Id = 4, Type = "Permission", Category = "role", Value = Permission.CanAssignRoleToUser.ToString(), Name = "Quyền gán vai trò cho người dùng" },
                new() { Id = 5, Type = "Permission", Category = "role", Value = Permission.CanRemoveRoleFromUser.ToString(), Name = "Quyền xóa vai trò khỏi người dùng" },

                // Claim Management
                new() { Id = 6, Type = "Permission", Category = "claim", Value = Permission.CanAddClaimToUser.ToString(), Name = "Quyền thêm claim cho người dùng" },
                new() { Id = 7, Type = "Permission", Category = "claim", Value = Permission.CanRemoveClaimFromUser.ToString(), Name = "Quyền xóa claim khỏi người dùng" },
                new() { Id = 8, Type = "Permission", Category = "claim", Value = Permission.CanAddClaimToRole.ToString(), Name = "Quyền thêm claim cho vai trò" },
                new() { Id = 9, Type = "Permission", Category = "claim", Value = Permission.CanRemoveClaimFromRole.ToString(), Name = "Quyền xóa claim khỏi vai trò" },

                // Category Management
                new() { Id = 10, Type = "Permission", Category = "category", Value = Permission.CanAddCategory.ToString(), Name = "Quyền thêm danh mục" },
                new() { Id = 11, Type = "Permission", Category = "category", Value = Permission.CanUpdateCategory.ToString(), Name = "Quyền cập nhật danh mục" },
                new() { Id = 12, Type = "Permission", Category = "category", Value = Permission.CanDeleteCategory.ToString(), Name = "Quyền xóa danh mục" },

                // Class Management
                new() { Id = 13, Type = "Permission", Category = "class", Value = Permission.CanGetStudentClass.ToString(), Name = "Quyền xem danh sách học viên trong lớp" },
                new() { Id = 14, Type = "Permission", Category = "class", Value = Permission.CanAddClass.ToString(), Name = "Quyền thêm lớp học" },
                new() { Id = 15, Type = "Permission", Category = "class", Value = Permission.CanUpdateClass.ToString(), Name = "Quyền cập nhật lớp học" },
                new() { Id = 16, Type = "Permission", Category = "class", Value = Permission.CanDeleteClass.ToString(), Name = "Quyền xóa lớp học" },
                new() { Id = 17, Type = "Permission", Category = "class", Value = Permission.CanDeleteStudentFromClass.ToString(), Name = "Quyền xóa học viên khỏi lớp" },

                // Course Management
                new() { Id = 18, Type = "Permission", Category = "course", Value = Permission.CanAddCourse.ToString(), Name = "Quyền thêm khóa học" },
                new() { Id = 19, Type = "Permission", Category = "course", Value = Permission.CanUpdateCourse.ToString(), Name = "Quyền cập nhật khóa học" },
                new() { Id = 20, Type = "Permission", Category = "course", Value = Permission.CanDeleteCourse.ToString(), Name = "Quyền xóa khóa học" },

                // Enrollment Management
                new() { Id = 21, Type = "Permission", Category = "enrollment", Value = Permission.CanEnrollStudent.ToString(), Name = "Quyền ghi danh học viên" },
                new() { Id = 22, Type = "Permission", Category = "enrollment", Value = Permission.CanUnenrollStudent.ToString(), Name = "Quyền hủy ghi danh học viên" },

                // Lesson Management
                new() { Id = 23, Type = "Permission", Category = "lesson", Value = Permission.CanAddLesson.ToString(), Name = "Quyền thêm bài học" },
                new() { Id = 24, Type = "Permission", Category = "lesson", Value = Permission.CanUpdateLesson.ToString(), Name = "Quyền cập nhật bài học" },
                new() { Id = 25, Type = "Permission", Category = "lesson", Value = Permission.CanDeleteLesson.ToString(), Name = "Quyền xóa bài học" },

                // Order Management
                new() { Id = 26, Type = "Permission", Category = "order", Value = Permission.CanGetAllOrders.ToString(), Name = "Quyền xem tất cả đơn hàng" },
                new() { Id = 27, Type = "Permission", Category = "order", Value = Permission.CanGetOrderStudent.ToString(), Name = "Quyền xem đơn hàng của học viên" },

                // Student Management
                new() { Id = 28, Type = "Permission", Category = "student", Value = Permission.CanAddStudent.ToString(), Name = "Quyền thêm học viên" },
                new() { Id = 29, Type = "Permission", Category = "student", Value = Permission.CanUpdateStudent.ToString(), Name = "Quyền cập nhật học viên" },
                new() { Id = 30, Type = "Permission", Category = "student", Value = Permission.CanDeleteStudent.ToString(), Name = "Quyền xóa học viên" },

                // Teacher Management
                new() { Id = 31, Type = "Permission", Category = "teacher", Value = Permission.CanAddTeacher.ToString(), Name = "Quyền thêm giáo viên" },
                new() { Id = 32, Type = "Permission", Category = "teacher", Value = Permission.CanUpdateTeacher.ToString(), Name = "Quyền cập nhật giáo viên" },
                new() { Id = 33, Type = "Permission", Category = "teacher", Value = Permission.CanDeleteTeacher.ToString(), Name = "Quyền xóa giáo viên" },

                // Attendance Management
                new() { Id = 34, Type = "Permission", Category = "attendance", Value = Permission.CanRecordAttendance.ToString(), Name = "Quyền ghi nhận điểm danh" },
                new() { Id = 35, Type = "Permission", Category = "attendance", Value = Permission.CanUpdateAttendance.ToString(), Name = "Quyền cập nhật điểm danh" },

                // Account Management
                new() { Id = 37, Type = "Permission", Category = "account", Value = Permission.CanCreateAccount.ToString(), Name = "Quyền tạo tài khoản" }
            };

            modelBuilder.Entity<ClaimEntity>().HasData(claims);


            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(
                Enum.GetValues(typeof(Permission))
                     .Cast<Permission>()
                     .Select((permission, index) => new IdentityRoleClaim<string>
                     {
                         Id = index + 1,
                         RoleId = "d2b1d8ad-6a8a-4675-ba92-4719e3d04ad8", // Đảm bảo đúng RoleId
                         ClaimType = "Permission",
                         ClaimValue = permission.ToString()
                     })
             );

        }
    }
}

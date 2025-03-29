using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum Permission
    {
        // Quản lý vai trò
        CanCreateRole,
        CanUpdateRole,
        CanDeleteRole,
        CanAssignRoleToUser,
        CanRemoveRoleFromUser,

        // Quản lý quyền (Claims)
        CanAddClaimToUser,
        CanRemoveClaimFromUser,
        CanAddClaimToRole,
        CanRemoveClaimFromRole,

        // Quản lý danh mục
        CanAddCategory,
        CanUpdateCategory,
        CanDeleteCategory,

        // Quản lý lớp học
        CanGetStudentClass,
        CanAddClass,
        CanUpdateClass,
        CanDeleteClass,
        CanDeleteStudentFromClass,

        // Quản lý khóa học
        CanAddCourse,
        CanUpdateCourse,
        CanDeleteCourse,

        // Quản lý ghi danh
        CanEnrollStudent,
        CanUnenrollStudent,

        // Quản lý bài học
        CanAddLesson,
        CanUpdateLesson,
        CanDeleteLesson,

        // Quản lý đơn hàng
        CanGetAllOrders,
        CanGetOrderStudent,

        // Quản lý học viên
        CanAddStudent,
        CanUpdateStudent,
        CanDeleteStudent,

        // Quản lý giáo viên
        CanAddTeacher,
        CanUpdateTeacher,
        CanDeleteTeacher,

        // Quản lý điểm danh
        CanRecordAttendance,
        CanUpdateAttendance,
        CanDeleteAttendance,

        // Quản lý tài khoản
        CanCreateAccount
    }

}

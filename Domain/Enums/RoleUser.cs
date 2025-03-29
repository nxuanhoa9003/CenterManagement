using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    /*public enum RoleUser
    {
        SuperAdmin,
        Admin,
        Employee,
        Student,
        Teacher
    }*/

    public class RoleUser
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Employee = "Employee";
        public const string Student = "Student";
        public const string Teacher = "Teacher";

        public static readonly string[] AllRoles = { SuperAdmin, Admin, Employee, Student, Teacher };
        public static IEnumerable<string> GetAllRoles() => AllRoles;
    }

}

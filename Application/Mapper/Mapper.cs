using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Shared.DTOs.Auth;
using Shared.DTOs.Category;
using Shared.DTOs.Class;
using Shared.DTOs.Course;
using Shared.DTOs.Enrollment;
using Shared.DTOs.Lesson;
using Shared.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UserDTO, ApplicationUser>()
               .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateTime.Parse(src.DateOfBirth))) // Convert string to DateTime
               .ReverseMap()
               .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.Value.ToString("yyyy-MM-dd")));

            CreateMap<CategoryRequest, Category>();
            CreateMap<ClassRequest, Class>();

            CreateMap<AttendanceRequest, Attendance>()
               .ForMember(dest => dest.Id, opt => opt.Ignore()) // Không map Id từ request
               .ForMember(dest => dest.AttendanceStatus, opt => opt.MapFrom(src => (AttendanceStatus)src.AttendanceStatus));


            CreateMap<LessonRequest, Lesson>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Không map Id từ request để tránh ghi đè
                .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.ClassId ?? Guid.Empty)); // Nếu ClassId null, gán Guid.Empty


            CreateMap<EnrollmentRequest, Enrollment>()
                .ForMember(dest => dest.EnrolledAt, opt => opt.MapFrom(_ => DateTime.UtcNow)); // Tự động gán ngày đăng ký hiện tại
            CreateMap<Enrollment, EnrollmentRequest>();

            CreateMap<CourseRequest, Course>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Bỏ qua Id, sẽ gán mới khi tạo
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()); // Không cập nhật CreatedDate tự động


            CreateMap<TeacherRequest, Teacher>()
                .ForMember(dest => dest.TeacherId, opt => opt.Ignore()); // Không map TeacherId từ request để tránh ghi đè
                
        }
    }
}

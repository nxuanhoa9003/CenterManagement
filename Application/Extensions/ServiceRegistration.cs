using Application.Services;
using Domain.Interface;
using Application.InterfacesServices;
using Infrastructures.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {

            // Đăng ký cloudinary
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            // đăng ký fluent mail
            services.AddTransient<IEmailService, EmailService>();

            // đăng ký vnpay
            services.AddTransient<IVnPayService, VnPayService>();

            // Đăng ký repository
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            // Đăng ký các dịch vụ
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IAuthorizationServices, AuthorizationService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IOtpService, OtpService>();
            services.AddTransient<IClassService, ClassService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IAttendanceService, AttendanceService>();
            services.AddTransient<ILessonService, LessonService>();
            services.AddTransient<IEnrollmentService, EnrollmentService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<ITeacherService, TeacherService>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IPaymentService, PaymentService>();


        }
    }
}

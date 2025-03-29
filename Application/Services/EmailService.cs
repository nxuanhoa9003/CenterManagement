using Application.InterfacesServices;
using FluentEmail.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Shared.DTOs.Email;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmail _fluentEmail;
        private readonly IWebHostEnvironment _env;
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings, IFluentEmail fluentEmail, IWebHostEnvironment env)
        {
            _fluentEmail = fluentEmail;
            _env = env;
            _mailSettings = mailSettings.Value;
        }

        public async Task<bool> SendEmailAsync(EmailRequest request)
        {
     
            string toEmail = request.ToEmail;
            string subject = request.Subject;
            string templateName = request.TemplateName;
            Dictionary<string, string> placeholders = request.Placeholders;


            // Đường dẫn đến file template HTML
            var templatePath = Path.Combine(_env.ContentRootPath, "Templates", $"{templateName}.html");

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Template {templateName}.html không tồn tại!");
            }
            // Đọc nội dung template HTML
            string emailBody = await File.ReadAllTextAsync(templatePath);


            emailBody = emailBody.Replace("{{subject}}", request.Subject);
            emailBody = emailBody.Replace("{{website_name}}", "test");

            // Thay thế placeholder bằng dữ liệu thực tế
            foreach (var placeholder in placeholders)
            {
                emailBody = emailBody.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }

            var email = await _fluentEmail
                        .To(toEmail)
                        .Subject(subject)
                        .Body(emailBody, true) // true để gửi HTML
                        .SendAsync();
            
            return email.Successful;
        }
    }
}

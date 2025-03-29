using Shared.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesServices
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailRequest request);
    }

}

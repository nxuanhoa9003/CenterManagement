using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InterfacesServices
{
    public interface IVnPayService
    {
        Task<string> CreatePaymentUrl(Order order);
        VnPaymentResponse PaymentExecute(IQueryCollection collections);
    }
}

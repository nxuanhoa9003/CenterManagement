using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Payment
{
    public class PaymentRequest
    {
        public Guid OrderId { get; set; }
    }
}

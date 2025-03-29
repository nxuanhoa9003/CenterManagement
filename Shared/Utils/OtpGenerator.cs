using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Utils
{
    public class OtpGenerator
    {
        public string OtpCode { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}

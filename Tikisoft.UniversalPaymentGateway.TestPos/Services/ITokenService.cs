using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tikisoft.UniversalPaymentGateway.TestPos.Services
{
    interface ITokenService
    {
        Task<string> GetTokenAsync();
    }
}

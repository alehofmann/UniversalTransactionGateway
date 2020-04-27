using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.ApiClient.Dto
{
    public class FeeDetailItemDto
    {
        public string FeePayer { get; set; }
        public decimal Amount { get; set; }
        public string FeeType { get; set; }

    }
}

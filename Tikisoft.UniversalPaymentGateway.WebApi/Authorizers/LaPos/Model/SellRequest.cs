using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Model
{
    public class SellRequest : BaseRequest
    {
      
        public decimal Amount { get; set; }
        public long InvoiceNumber { get; set; }
        public int Installments { get; set; }
        public string CardCode { get; set; } = "";
        public int PlanCode { get; set; }
        public decimal TipAmount { get; set; }
        public long MerchantNumber { get; set; }
        public string MerchantName { get; set; } = "";
        public string MerchantCuit { get; set; } = "";
        public bool IsOnlineTransaction { get; set; }

    }
}

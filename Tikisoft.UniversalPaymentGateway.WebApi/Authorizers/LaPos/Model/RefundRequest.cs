using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.LaPos.Model
{
    public class RefundRequest : BaseRequest
    {
        public decimal Amount { get; set; }
        public string CardCode { get; set; }
        public int PlanCode { get; set; }
        public int Installments { get; set; }
        public int OriginalVoucher { get; set; }
        public DateTime OriginalTransactionDate { get; set; }
        public int InvoiceNumber { get; set; }
        public int MerchantNumber { get; set; }
        public string MerchantName { get; set; }
        public string MerchantCuit { get; set; }
        public bool IsOnlineTransaction { get; set; }

    }
}

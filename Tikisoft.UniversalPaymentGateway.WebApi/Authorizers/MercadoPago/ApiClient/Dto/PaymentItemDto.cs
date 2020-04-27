using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.ApiClient.Dto
{
    public class PaymentItemDto
    {
        public DateTime MoneyReleaseDate {get;set;}
        public string Payment_MethodId { get; set; }
        public string Payment_TypeId { get; set; }
        public string Payment_Status { get; set; }
        public string Payment_Status_Detail { get; set; }
        public string Payer_Email { get; set; }
        public string Payer_FirstName { get; set; }
        public string Payer_LastName { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal NetReceivedAmount { get; set; }
        public int Installments { get; set; }
        public decimal InstallmentAmount { get; set; }
        public string Card_FirstSix { get; set; }
        public string Card_LastFour { get; set; }
        public string AuthCode { get; set; }
        public string ExtReference { get; set; }
        public string PaymentType { get; set; }
        public IList<FeeDetailItemDto> FeeDetail { get; set; } = new List<FeeDetailItemDto>();

    }
}

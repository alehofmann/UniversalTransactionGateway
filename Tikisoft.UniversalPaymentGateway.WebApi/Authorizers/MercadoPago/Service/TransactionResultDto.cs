using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.Service
{
    public class TransactionResultDto
    {
        public string Message { get; set; }
        public int ErrorCode { get; set; }
        public string CardNumber { get; set; }
        public string Branch { get; set; }
        public string PaymentType { get; set; }
        public string TransactionId { get; set; }
        public string TransactionReference { get; set; }

        public TransactionResultDto(string errorMessage, int errorCode)
        {
            Message = errorMessage;
            ErrorCode = errorCode;
        }

        public TransactionResultDto(string msg, int finishCode, string card, string branch, string payment, string transId,
            string transRef)
        {
            Message = msg;
            ErrorCode = finishCode;
            CardNumber = card;
            Branch = branch;
            PaymentType = payment;
            TransactionId = transId;
            TransactionReference = transRef;
        }
    }
}

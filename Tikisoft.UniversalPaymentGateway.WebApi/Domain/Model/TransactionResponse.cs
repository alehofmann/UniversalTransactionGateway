using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Domain.Model
{
    public enum PaymentType
    {
        CreditCard = 1,
        DebitCard = 2,
        PrepaidCard = 3,
        AccountMoney = 4,
        Ticket = 5,
        BankTransfer = 6,
        Atm = 6
    }

    public class TransactionResponse : BaseResponse
    {
    
        public TransactionResponse(ResultCodesEnum resultCode, IList<string> errorList = null) : base(resultCode, errorList) { }
        public TransactionResponse(ResultCodesEnum resultCode, string errorDescription) : base(resultCode, errorDescription) { }
        public TransactionResponse() { }
        public PaymentType PaymentType { get; set; }
        public string PaymentTypeString => this.PaymentType.ToString();
        public CardInfo CardInfo { get; set; }
        public PayerInfo PayerInfo { get; set; }
        public decimal TotalPaid { get; set; }
        public DateTime MoneyReleaseDate { get; set; }
        public decimal NetReceived { get; set; }
        public IList<FeeDetailItem> FeeDetails { get; set; }
        public string AuthCode { get; set; }
        public string ExternalReference { get; set; }
        public string TerminalId { get; set; }
        public string VoucherCode { get; set; }
        public decimal TotalFees
        {
            get
            {
                return FeeDetails != null ? FeeDetails.Sum(i => i.Amount):0;
            }
        }
                
    }
}

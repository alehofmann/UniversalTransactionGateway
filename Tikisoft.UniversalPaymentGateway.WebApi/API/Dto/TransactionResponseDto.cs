using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.Domain.Model;

namespace TikiSoft.UniversalPaymentGateway.API.Dto
{
    public class TransactionResponseDto
    {
        public TransactionResponseDto(TransactionResponse response)
        {
            PaymentTypeString = response.PaymentTypeString;
            ResultCodeString = response.ResultCodeString;
            ResultDescription = response.ResultDescription;
            CardInfo = response.CardInfo;
            PayerInfo = response.PayerInfo;
            TotalPaid = response.TotalPaid;
            MoneyReleaseDate = response.MoneyReleaseDate;
            NetReceived = response.NetReceived;
            FeeDetails = response.FeeDetails;
            AuthCode = response.AuthCode;
            ExternalReference = response.ExternalReference;
            TotalFees = response.TotalFees;
            TerminalId = response.TerminalId;
            VoucherCode = response.VoucherCode;
        }
        [JsonProperty("success")]
        public bool Success { get { return true; } }

        [JsonProperty("result_code")]
        public string ResultCodeString { get; set; }

        [JsonProperty("result_description")]
        public string ResultDescription { get; set; }

        [JsonProperty("voucher_code")]
        public string VoucherCode { get; set; }

        [JsonProperty("terminal_id")]
        public string TerminalId { get; set; }

        [JsonProperty("payment_type", Required = Required.Always)]
        public string PaymentTypeString { get; set; }

        [JsonProperty("card_info")]
        public CardInfo CardInfo { get; set; }

        [JsonProperty("payer_info")]
        public PayerInfo PayerInfo { get; set; }

        [JsonProperty("total_paid")]
        public decimal TotalPaid { get; set; }

        [JsonProperty("money_release_date")]
        public DateTime MoneyReleaseDate { get; set; }

        [JsonProperty("net_received")]
        public decimal NetReceived { get; set; }

        [JsonProperty("fee_details")]
        public IList<FeeDetailItem> FeeDetails { get; set; }

        [JsonProperty("auth_code")]
        public string AuthCode { get; set; }

        [JsonProperty("external_reference")]
        public string ExternalReference { get; set; }

        [JsonProperty("total_fees")]
        public decimal TotalFees { get; set; }
    }
}

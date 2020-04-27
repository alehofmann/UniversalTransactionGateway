using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Domain.Model
{
    public enum TransactionType
    {
        [Description("sell")]
        Sell = 1,
        [Description("void")]
        Void = 2,
        [Description("return")]
        Return = 3
    }

    public class TransactionRequest
    {
        public TransactionRequest(TransactionType transType)
        {
            TransType = transType;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TransDefId { get; set; }
        public TransactionType TransType { get; set; }                
        public decimal Amount { get; set; }
        public decimal Gratuity { get; set; }
        public string TransactionReference { get; set; }
        public IList<TransItem> Items { get; set; }
        public string PosId { get; set; }
        public string CardCode { get; set; }
        public int Installments { get; set; }
        public long InvoiceNumber { get; set; }
        public decimal TipAmount { get; set; }
        public int MerchantId { get; set; } = 0;
    }

    
}

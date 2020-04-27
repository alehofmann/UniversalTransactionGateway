using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.Models
{
    [Table("MercadoPagoTransaction")]
    public class TransactionRecord
    {
        [Key]
        [Column("CreditCardTransaction_Id")]
        public long Id { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal Amount { get; set; }
        public string ProcessorType { get; set; }
        public byte TransactionType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TikiSoft.UniversalPaymentGateway.Authorizers.MercadoPago.Models
{
    [Table("MercadoPagoNotifications")]
    public class NotificationRecord
    {
        public enum NotificationType
        {
            Payment = 1,
            MerchantOrder = 2
        }

        [Key]
        [Column("MercadoPagoNotification_Id")]
        public long Id { get; set; }
        
        [Column("External_Id")]
        public long ExternalId { get; set; }
        public NotificationType Type { get; set; }
        public DateTime DateReceived { get; set; }
        public long LogCreditCardTransactionId { get; set; }
        public string Status { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentType { get; set; }
        public string LastCardDigits { get; set; }

    }
}
